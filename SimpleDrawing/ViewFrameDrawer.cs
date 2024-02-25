﻿using System.Diagnostics;
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

  private Shader _shader;
  private Shader _lampShader;


  DirectionalLight _dirLight;
  PointLight _pointLight;
  FlashLight _flashLight;

  private int _lampCount = 0;

  private Matrix4 _view;
  private Matrix4 _projection;
  private readonly float _FOV = 45.0f;

  // first "_lampCount" volumes are the lamp's volumes
  private List<Volume> _volumes = new List<Volume>();

  private List<int> _vertexBufferObjects;
  private List<int> _normalBufferObjects;
  private List<int> _vertexArrayObjects;

  private Show _showType;

  private float _lastTimestamp = Stopwatch.GetTimestamp();
  private float _interpolationKoeff;
  private bool _increase;

  Vector3 _facesColor;
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

    _vertexBufferObjects = new List<int>();
    _normalBufferObjects = new List<int>();
    _vertexArrayObjects = new List<int>();

    _increase = true;
    _interpolationKoeff = 0.2f;

    _view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 10.0f), new Vector3(1.5f, 2.0f, 0.0f), Vector3.UnitY);
    _projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * (_FOV / 180f),
        _width / (float)_height, 0.2f, 256.0f);

    _volumes = new List<Volume>();


    // ////////////////////////////////////////////////////////////////////////////////////////////////////////
    _dirLight = new DirectionalLight();
    _dirLight.Direction = new Vector3(1.0f, -1.0f, 1.0f);
    ++_lampCount;

    _pointLight = new PointLight();
    ++_lampCount;

    _flashLight = new FlashLight();
    ++_lampCount;

    _volumes.Add(_dirLight._form);
    _volumes.Add(_pointLight._form);
    _volumes.Add(_flashLight._form);
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////


    var cubes = Generator.GenerateVolumes();
    _volumes.AddRange(cubes);

    if (_volumes.Count > _lampCount) {
      _facesColor = _volumes[_lampCount].MaterialTraits.Ambient;

    } else {
      _facesColor = _volumes[0].MaterialTraits.Ambient;
    }

    _edgesColor = _facesColor;
    _pointsColor = _facesColor;


  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public void OnLoad() {
    ChangeDrawingType(0, true);
    GL.Enable(EnableCap.ProgramPointSize);

    for (int i = 0; i < _volumes.Count; ++i) {
      _vertexArrayObjects.Add(GL.GenVertexArray());
      GL.BindVertexArray(_vertexArrayObjects[i]);

      if (_volumes[i].Vertices != null) {
        BindPosBuffer(i);
      }

      if (_volumes[i].Normals != null) {
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
    _shader.SetMatrix4("view", _view);
    _shader.SetMatrix4("projection", _projection);
// dirlight light
    _shader.SetUniform3($"dirlights[{0}].direction", _dirLight.Direction);
    _shader.SetUniform3($"dirlights[{0}].color", _dirLight.Color);
    _shader.SetUniform3($"dirlights[{0}].diffuse", _dirLight.Diffuse);
    _shader.SetUniform3($"dirlights[{0}].specular", _dirLight.Specular);
// pointlight
    _shader.SetUniform3($"pointLights[{0}].position", _pointLight._form.PosVr);
    _shader.SetUniform3($"pointLights[{0}].color", _pointLight.Color);
    _shader.SetUniform3($"pointLights[{0}].diffuse", _pointLight.Diffuse);
    _shader.SetUniform3($"pointLights[{0}].specular", _pointLight.Specular);
    _shader.SetFloat($"pointLights[{0}].constant", _pointLight.Constant);
    _shader.SetFloat($"pointLights[{0}].linear", _pointLight.Linear);
    _shader.SetFloat($"pointLights[{0}].quadratic", _pointLight.Quadratic);
// flashlight
    _shader.SetUniform3($"flashLights[{0}].position", _flashLight._form.PosVr);
    _shader.SetUniform3($"flashLights[{0}].direction", _flashLight.Direction);
    _shader.SetFloat($"flashLights[{0}].cutOff", _flashLight.CutOff);
    _shader.SetFloat($"flashLights[{0}].outerCutOff", _flashLight.OuterCutOff);
    _shader.SetUniform3($"flashLights[{0}].color", _flashLight.Color);
    _shader.SetUniform3($"flashLights[{0}].diffuse", _flashLight.Diffuse);
    _shader.SetUniform3($"flashLights[{0}].specular", _flashLight.Specular);
    _shader.SetFloat($"flashLights[{0}].constant", _flashLight.Constant);
    _shader.SetFloat($"flashLights[{0}].linear", _flashLight.Linear);
    _shader.SetFloat($"flashLights[{0}].quadratic", _flashLight.Quadratic);
/*
*/

    for (int i = _lampCount; i < _volumes.Count; ++i) {

      Matrix4 model = _volumes[i].ComputeModelMatrix();
      _shader.SetMatrix4("model", model);

      model.Invert();
      _shader.SetMatrix4("invertedModel", model);


      _shader.SetUniform3("material.ambient", _volumes[i].MaterialTraits.Ambient);
      _shader.SetUniform3("material.diffuse", _volumes[i].MaterialTraits.Diffuse);
      _shader.SetUniform3("material.specular", _volumes[i].MaterialTraits.Specular);
      _shader.SetFloat("material.shiness", _volumes[i].MaterialTraits.Shiness);
     
      GL.BindVertexArray(_vertexArrayObjects[i]);

      _showType(i);  // delegate
    }
  }

  void ShowLamps() {

    _lampShader.SetMatrix4("view", _view);
    _lampShader.SetMatrix4("projection", _projection);

    for (int i = 0; i < _lampCount; ++i) {

      Matrix4 modelLamp = _volumes[i].ComputeModelMatrix();
      _lampShader.SetMatrix4("model", modelLamp);
      _lampShader.SetUniform3("aColor", _volumes[i].MaterialTraits.Ambient);
      GL.BindVertexArray(_vertexArrayObjects[i]);

      ShowSolid(i);

    }
  }

  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  private void BindPosBuffer(int indexOfDescriptros) {
    _vertexBufferObjects.Add(GL.GenBuffer());
    int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPos");
    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjects[indexOfDescriptros]);
    GL.BufferData(BufferTarget.ArrayBuffer,
        _volumes[indexOfDescriptros].Vertices.Length * sizeof(float),
        _volumes[indexOfDescriptros].Vertices, BufferUsageHint.DynamicDraw);
    GL.EnableVertexAttribArray(vertexLocation);
    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }

  private void BindNormalBuffer(int indexOfDescriptros) {
    _normalBufferObjects.Add(GL.GenBuffer());
    int normalLocation = GL.GetAttribLocation(_shader.Handle, "aNormal");
    GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBufferObjects[indexOfDescriptros]);
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
    if (i >= _lampCount) {
      _shader.SetUniform3("material.ambient", _facesColor);
    }

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
  public void ChangeFacesColor(Vector3 color) {
    _facesColor = color;
  }

  public void ChangeEdgesColor(Vector3 color) {
    _edgesColor = color;
  }

  public void ChangePointsColor(Vector3 color) {
    _pointsColor = color;
  }
  //  //////////////////////////////////////////////////////////////////////////////



  //  //////////////////////////////////////////////////////////////////////////////
  public void SetMatrices() {
    _view = _camera.GetViewMatrix();
    _projection = _camera.GetProjectionMatrix();
  }


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

