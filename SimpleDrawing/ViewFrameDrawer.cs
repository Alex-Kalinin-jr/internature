using System.Diagnostics;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SimpleDrawing.Entities;

namespace SimpleDrawing;

public delegate void Show(int i);

public sealed class SceneDrawer {
  private int _width;
  private int _height;

  Camera _camera;
  const float _cameraSpeed = 1.5f;
  private readonly float _FOV = 45.0f;

  private Shader _shader;
  private Shader _lampShader;

  private List<Volume> _volumes;
  private List<Light> _lights;

  private List<int> _normalBufferObjects;
  private List<int> _vertexArrayObjects;

  private List<int> _lightsBufferObjects;
  private List<int> _normalLightsBufferObjects;
  private List<int> _lightsArrayObjects;

  private Show _showType;

  private float _lastTimestamp = Stopwatch.GetTimestamp();
  private float _interpolationKoeff;
  private bool _increase;

  Vector3 _edgesColor;
  Vector3 _pointsColor;
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public SceneDrawer() {
    _width = 1024;
    _height = 768;
    _shader = new Shader("Shader/Shaders/shader.vert", "Shader/Shaders/shader.frag");
    _lampShader = new Shader("Shader/Shaders/lightShader.vert", "Shader/Shaders/lightShader.frag");

    _camera = new Camera(Vector3.UnitZ * 3, _width / _height);

    _vertexArrayObjects = new List<int>();

    _increase = true;
    _interpolationKoeff = 0.2f;

    _edgesColor = new Vector3(0.0f, 0.0f, 0.0f);
    _pointsColor = new Vector3(0.0f, 0.0f, 0.0f);

    _volumes = new List<Volume>();
    _lights = new List<Light>();

  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public void OnLoad() {

    // theese code should be in some another place /////////////////
    var dirLight = new DirectionalLight();
    dirLight.Direction = new Vector3(0.0f, -1.0f, 0.0f);
    var pointLight = new PointLight();
    var flashLight = new FlashLight();
    flashLight.Direction = new Vector3(0.0f, 0.3f, -1.0f);
    _lights.Add(dirLight);
    _lights.Add(pointLight);
    _lights.Add(flashLight);

    var cubes = Generator.GenerateVolumes();
    _volumes.AddRange(cubes);
    // theese code should be in some another place /////////////////


    ChangeDrawingType(0, true);
    GL.Enable(EnableCap.ProgramPointSize);

    for (int i = 0; i < _volumes.Count; ++i) {

      _vertexArrayObjects.Add(GL.GenVertexArray());
      GL.BindVertexArray(_vertexArrayObjects[i]);

      if (_volumes[i].Vertices != null && _volumes[i].Normals != null) {
        BindPosBuffer(i);
        BindNormalBuffer(i);
      }

    }

    GL.Enable(EnableCap.DepthTest);
    GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
  }

  public void OnResize(int width, int height) {
    _width = width;
    _height = height;

    GL.Viewport(0, 0, _width, _height);
  }

  public void OnRenderFrame() {

    ChangeBlend(); // morph component changing

    ShowVolumes();
    ShowLamps();

  }

  public void OnClosed() { }
  //  //////////////////////////////////////////////////////////////////////////////




  //  //////////////////////////////////////////////////////////////////////////////
  void ShowVolumes() {

    _shader.SetFloat("morphingFactor", _interpolationKoeff);
    _shader.SetUniform3("viewPos", _camera.Position);
    _shader.SetMatrix4("view", _camera.GetViewMatrix());
    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

    for (int i = 0; i < _lights.Count; ++i) {
      _lights[i].AdjustShader(ref _shader, 0);
    }

    for (int i = 0; i < _volumes.Count; ++i) {

      _volumes[i].AdjustShader(ref _shader);
      
      GL.BindVertexArray(_vertexArrayObjects[i]);

      _showType(i);
    }
  }

  void ShowLamps() {

    _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
    _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

    for (int i = 0; i < _lights.Count; ++i) {

      Matrix4 modelLamp = _lights[i]._form.ComputeModelMatrix();
      _lampShader.SetMatrix4("model", modelLamp);
      _lampShader.SetUniform3("aColor", _volumes[i].MaterialTraits.Ambient);
      GL.BindVertexArray(_vertexArrayObjects[i]);

      ShowSolid(i);

    }
  }

  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  private void BindPosBuffer(int indexOfDescriptros) {
    int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPos");
    GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
    GL.BufferData(BufferTarget.ArrayBuffer,
        _volumes[indexOfDescriptros].Vertices.Length * sizeof(float),
        _volumes[indexOfDescriptros].Vertices, BufferUsageHint.DynamicDraw);
    GL.EnableVertexAttribArray(vertexLocation);
    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }

  private void BindNormalBuffer(int indexOfDescriptros) {
    int normalLocation = GL.GetAttribLocation(_shader.Handle, "aNormal");
    GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
    GL.BufferData(BufferTarget.ArrayBuffer,
      _volumes[indexOfDescriptros].Normals.Length * sizeof(float),
      _volumes[indexOfDescriptros].Normals, BufferUsageHint.StaticDraw);
    GL.EnableVertexAttribArray(normalLocation);
    GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }





  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  private void ShowSolid(int i) {
    GL.DrawArrays(PrimitiveType.Triangles, 0, _volumes[i].Vertices.Length / 3);
  }

  private void ShowFramed(int i) {
    _shader.SetUniform3("material.ambient", _edgesColor);

    int bias = 0;
    int step = _volumes[i].Vertices.Length / 3 / 6;

    for (int g = 0; g < 6; ++g) {
      GL.DrawArrays(PrimitiveType.LineStrip, bias, step);
      bias += step;
    }
  }

  private void ShowPoints(int i) {
    _shader.SetUniform3("material.ambient", _pointsColor);
    GL.DrawArrays(PrimitiveType.Points, 0, _volumes[i].Vertices.Length / 3);
  }

  public void ChangeDrawingType(int i, bool state) {

    if (i == 0) {
      if (state) {
        _showType += ShowSolid;
      } else {
        _showType -= ShowSolid;
      }
    }

    if (i == 1) {
      if (state) {
        _showType += ShowFramed;
      } else {
        _showType -= ShowFramed;
      }
    }

    if (i == 2) {
      if (state) {
        _showType += ShowPoints;
      } else {
        _showType -= ShowPoints;
      }
    }
  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public void ChangeEdgesColor(Vector3 color) {
    _edgesColor = color;
  }

  public void ChangePointsColor(Vector3 color) {
    _pointsColor = color;
  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  private void ChangeBlend() {
    float step = 0.005f;
    if (_increase) {
      _interpolationKoeff += step;
    } else {
      _interpolationKoeff -= step;
    }

    if (_interpolationKoeff >= 1.0f || _interpolationKoeff <= 0.0f) {
      _increase = _increase ^ true;
    }
  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public void MoveCameraFwd(float val) {
    _camera.Position += _camera.Front * _cameraSpeed * val;
  }

  public void MoveCameraBack(float val) {
    _camera.Position -= _camera.Front * _cameraSpeed * val;
  }

  public void MoveCameraRight(float val) {
    _camera.Position -= _camera.Right * _cameraSpeed * val;
  }

  public void MoveCameraLeft(float val) {
    _camera.Position += _camera.Right * _cameraSpeed * val;
  }

  public void MoveCameraDown(float val) {
    _camera.Position += _camera.Up * _cameraSpeed * val;
  }

  public void MoveCameraUp(float val) {
    _camera.Position -= _camera.Up * _cameraSpeed * val;
  }

  public void ChangeCameraPitch(float val) {
    _camera.Pitch += val;
  }

  public void ChangeCameraYaw(float val) {
    _camera.Yaw += val;
  }

}

