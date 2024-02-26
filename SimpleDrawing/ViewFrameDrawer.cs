using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SimpleDrawing.Entities;

namespace SimpleDrawing;

public delegate void Show(int length);

public sealed class SceneDrawer {
  private int _width;
  private int _height;

  Camera _camera;
  const float _cameraSpeed = 1.5f;

  private Shader _shader;
  private Shader _lampShader;

  private List<Volume> _volumes;
  private List<Light> _lights;

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
    _increase = true;
    _interpolationKoeff = 0.2f;
    _edgesColor = new Vector3(0.0f, 0.0f, 0.0f);
    _pointsColor = new Vector3(0.0f, 0.0f, 0.0f);
    _volumes = new List<Volume>();
    _lights = new List<Light>();
  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  private void BindPosBuffer(float[] vertices) {
    int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPos");
    GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
        vertices, BufferUsageHint.DynamicDraw);
    GL.EnableVertexAttribArray(vertexLocation);
    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }

  private void BindNormalBuffer(float[] normals) {
    int normalLocation = GL.GetAttribLocation(_shader.Handle, "aNormal");
    GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
    GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float),
        normals, BufferUsageHint.StaticDraw);
    GL.EnableVertexAttribArray(normalLocation);
    GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }
  //  //////////////////////////////////////////////////////////////////////////////


  //  //////////////////////////////////////////////////////////////////////////////
  public void OnLoad() {
    _volumes.AddRange(Generator.GenerateVolumes());
    _lights.AddRange(Generator.GenerateLights());
    ChangeDrawingType(0, true);

    GL.Enable(EnableCap.ProgramPointSize);
    GL.Enable(EnableCap.DepthTest);
    GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

    for (int i = 0; i < _volumes.Count; ++i) {
      _volumes[i].VAO = GL.GenVertexArray();
      GL.BindVertexArray(_volumes[i].VAO);

      if (_volumes[i].Vertices != null) {
        BindPosBuffer(_volumes[i].Vertices);
      }

      if (_volumes[i].Normals != null) {
        BindNormalBuffer(_volumes[i].Normals);
      }
    }

    for (int i =0; i < _lights.Count; ++i) {
      _lights[i].Form.VAO = GL.GenVertexArray();
      GL.BindVertexArray(_lights[i].Form.VAO);

      if (_lights[i].Form.Vertices != null) {
        BindPosBuffer(_lights[i].Form.Vertices);
      }

      if (_lights[i].Form.Normals != null) {
        BindNormalBuffer(_lights[i].Form.Normals);
      }
    }


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
      GL.BindVertexArray(_volumes[i].VAO);
      _showType(_volumes[i].Vertices.Length);
    }
  }

  void ShowLamps() {

    _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
    _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

    for (int i = 0; i < _lights.Count; ++i) {
      Matrix4 modelLamp = _lights[i].Form.ComputeModelMatrix();
      _lampShader.SetMatrix4("model", modelLamp);
      _lampShader.SetUniform3("aColor", _lights[i].Form.MaterialTraits.Ambient);
      GL.BindVertexArray(_lights[i].Form.VAO);
      GL.DrawArrays(PrimitiveType.Triangles, 0, _lights[i].Form.Vertices.Length / 3);
    }
  }
  //  //////////////////////////////////////////////////////////////////////////////




  //  //////////////////////////////////////////////////////////////////////////////
  private void ShowSolid(int length) {
    GL.DrawArrays(PrimitiveType.Triangles, 0, length / 3);
  }

  private void ShowFramed(int length) {
    _shader.SetUniform3("material.ambient", _edgesColor);

    int bias = 0;
    int step = length / 3 / 6;

    for (int g = 0; g < 6; ++g) {
      GL.DrawArrays(PrimitiveType.LineStrip, bias, step);
      bias += step;
    }
  }

  private void ShowPoints(int length) {
    _shader.SetUniform3("material.ambient", _pointsColor);
    GL.DrawArrays(PrimitiveType.Points, 0, length / 3);
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
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  internal void ChangeDirLight(DirectionalLight val) {
    val.Form.VAO = _lights[0].Form.VAO;
    _lights[0] = val;
  }

  internal void ChangePointLight(PointLight val) {
    val.Form.VAO = _lights[1].Form.VAO;
    _lights[1] = val;
  }

  internal void ChangeFlashLight(FlashLight val) {
    val.Form.VAO = _lights[2].Form.VAO;
    _lights[2] = val;
  }
}

