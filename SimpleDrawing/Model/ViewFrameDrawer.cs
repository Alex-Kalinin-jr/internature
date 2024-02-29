﻿using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {
  public delegate void Show(int length);

  public sealed class SceneDrawer {
    const float _cameraSpeed = 1.5f;

    private int _width;
    private int _height;

    private Camera _camera;
    private Letters _letters;
    private List<Volume> _volumes;
    private List<Light> _lights;

    private Shader _shader;
    private Shader _lampShader;
    private Shader _lettersShader;

    private Show _showType;

    private string _renderTime;
    private float _interpolationKoeff;
    private bool _increase;

    private OpenTK.Mathematics.Vector3 _edgesColor;
    private OpenTK.Mathematics.Vector3 _pointsColor;


    // test
    CircleShiftingMover _circleShiftingMover;
    RotaterMover _rotateaterMover;
    UpDownMover _upDownMover;
    // end of test

    //  //////////////////////////////////////////////////////////////////////////////////////



    //  //////////////////////////////////////////////////////////////////////////////////////
    public SceneDrawer() {
      _width = 1024;
      _height = 768;

      _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _lampShader = new Shader("Shaders/lightShader.vert", "Shaders/lightShader.frag");
      _lettersShader = new Shader("Shaders/LetterShader.vert", "Shaders/LetterShader.frag");

      _camera = new Camera(OpenTK.Mathematics.Vector3.UnitZ * 3, _width / _height);
      _increase = true;
      _interpolationKoeff = 0.2f;
      _edgesColor = new OpenTK.Mathematics.Vector3(0.0f, 0.0f, 0.0f);
      _pointsColor = new OpenTK.Mathematics.Vector3(0.0f, 0.0f, 0.0f);
      _volumes = new List<Volume>();
      _lights = new List<Light>();
      _letters = new Letters(0.05f, 0.1f);

      _circleShiftingMover = new CircleShiftingMover(1000);
      _rotateaterMover = new RotaterMover();
      _upDownMover = new UpDownMover();
      
    }
    //  //////////////////////////////////////////////////////////////////////////////////////



    //  //////////////////////////////////////////////////////////////////////////////////////
    public void OnLoad() {

      _volumes.AddRange(Generator.GenerateVolumes());
      _lights.AddRange(Generator.GenerateLights());
      ChangeDrawingType(0, true);

      GL.Enable(EnableCap.ProgramPointSize);
      GL.Enable(EnableCap.DepthTest);
      GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

      for (int i = 0; i < _volumes.Count; ++i) {
        _volumes[i].Vao = GL.GenVertexArray();
        GL.BindVertexArray(_volumes[i].Vao);

        if (_volumes[i].Vertices != null) {
          BindPosBuffer(_volumes[i].Vertices);
        }

        if (_volumes[i].Normals != null) {
          BindNormalBuffer(_volumes[i].Normals);
        }
      }

      for (int i = 0; i < _lights.Count; ++i) {
        _lights[i].Form.Vao = GL.GenVertexArray();
        GL.BindVertexArray(_lights[i].Form.Vao);

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
      MoveVolumes();
      ShowVolumes();
      ShowLamps();
      _letters.DrawFps(ref _lettersShader, "FPS" + _renderTime);

    }

    public void OnClosed() { }
    //  //////////////////////////////////////////////////////////////////////////////////////

    public void SetTime(string time) {
      _renderTime = time;
    }

    //  //////////////////////////////////////////////////////////////////////////////////////
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

    public void ChangeEdgesColor(OpenTK.Mathematics.Vector3 color) {
      _edgesColor = color;
    }

    public void ChangePointsColor(OpenTK.Mathematics.Vector3 color) {
      _pointsColor = color;
    }

    public void MoveCameraFwd(float val) {
      _camera.Position += _camera.Front * _cameraSpeed * val;
    }

    public void MoveCameraBack(float val) {
      _camera.Position -= _camera.Front * _cameraSpeed * (val);
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

    public void ChangeDirLight(DirectionalLight val) {
      val.Form.Vao = _lights[0].Form.Vao;
      _lights[0] = val;
    }

    public void ChangePointLight(PointLight val) {
      val.Form.Vao = _lights[1].Form.Vao;
      _lights[1] = val;
    }

    public void ChangeFlashLight(FlashLight val) {
      val.Form.Vao = _lights[2].Form.Vao;
      _lights[2] = val;
    }
    //  //////////////////////////////////////////////////////////////////////////////////////



    //  //////////////////////////////////////////////////////////////////////////////////////
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


    private void MoveVolumes() {
      for (int i = 0; i < _volumes.Count; ++i) {
        var volume = _volumes[i];
        MoveVolume(ref volume);
        _volumes[i] = volume;
      }

      for (int i = 2; i < _lights.Count; ++i) {
        var light = _lights[i].Form;
        MoveVolume(ref light);
        _lights[i].Form = light;
      }
    }

    private void MoveVolume(ref Volume volume) {
      _circleShiftingMover.Move(ref volume);
      _rotateaterMover.Move(ref volume);
      _upDownMover.Move(ref volume);
    }

    private void ShowVolumes() {
      _shader.SetFloat("morphingFactor", _interpolationKoeff);
      _shader.SetUniform3("viewPos", _camera.Position);
      _shader.SetMatrix4("view", _camera.GetViewMatrix());
      _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      for (int i = 0; i < _lights.Count; ++i) {
        if (i < 2) {
          _lights[i].AdjustShader(ref _shader, 0);
        } else {
          _lights[i].AdjustShader(ref _shader, i - 2);
        }
      }

      for (int i = 0; i < _volumes.Count; ++i) {
        _volumes[i].AdjustShader(ref _shader);
        GL.BindVertexArray(_volumes[i].Vao);
        _showType(_volumes[i].Vertices.Length);
      }
    }

    private void ShowLamps() {

      _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      for (int i = 0; i < _lights.Count; ++i) {
        OpenTK.Mathematics.Matrix4 modelLamp = _lights[i].Form.ComputeModelMatrix();
        _lampShader.SetMatrix4("model", modelLamp);
        _lampShader.SetUniform3("aColor", _lights[i].Form.MaterialTraits.Ambient);
        GL.BindVertexArray(_lights[i].Form.Vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _lights[i].Form.Vertices.Length / 3);
      }
    }
  }
}



