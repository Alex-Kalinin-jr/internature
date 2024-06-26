﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public delegate void Show(int length);
  public delegate void MoveVolume(ref Position vol);

  public sealed class SceneDrawer {
    const float _cameraSpeed = 1.5f;

    private int _width;
    private int _height;

    private Camera _camera;
    private Letters _letters;
    private List<Volume> _volumes;

    private List<Light> _lights;
    private List<Light> _pointLights;
    private List<Light> _directionalLights;

    private Shader _shader;
    private Shader _lampShader;
    private Shader _lettersShader;

    private Show _showType;
    private MoveVolume _moveVolume;
    private MoveVolume _moveFlashLight;

    private string _renderTime;
    private float _interpolationKoeff;
    private bool _increase;

    CircleShiftingMover _circleShiftingMover;
    RotationMover _rotateaterMover;
    UpDownMover _upDownMover;

//  //////////////////////////////////////////////////////////////////////////////////////

    public SceneDrawer() {
      _width = 1024;
      _height = 768;

      _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _lampShader = new Shader("Shaders/lightShader.vert", "Shaders/lightShader.frag");
      _lettersShader = new Shader("Shaders/LetterShader.vert", "Shaders/LetterShader.frag");

      _camera = new Camera(Vector3.UnitZ * 3, _width / _height);
      _increase = true;
      _interpolationKoeff = 0.2f;
      _volumes = new List<Volume>();
      _lights = new List<Light>();
      _pointLights = new List<Light>();
      _directionalLights = new List<Light>();
      _letters = new Letters(0.05f, 0.1f); // just an example of width and height

      _circleShiftingMover = new CircleShiftingMover(1000);
      _rotateaterMover = new RotationMover();
      _upDownMover = new UpDownMover();
      
    }


    public void OnLoad() {

      _volumes.AddRange(Generator.GenerateVolumes(10, 2.0f));
      (_directionalLights, _pointLights, _lights ) = Generator.GenerateLights(3, 2.0f);

      ChangeDrawingType(0, true);

      GL.Enable(EnableCap.ProgramPointSize);
      GL.Enable(EnableCap.DepthTest);
      GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

      CreateAndBindVolumesBuffers();
      CreateAndBindLampsBuffers();

    }

    public void OnResize(int width, int height) {
      _width = width;
      _height = height;

      GL.Viewport(0, 0, _width, _height);
    }

    public void OnRenderFrame() {

      ChangeBlend(); // morph component changing
      MoveVolumes();
      MoveLights();

      ShowVolumes();
      ShowLamps(_lights);
      ShowLamps(_directionalLights);
      ShowLamps(_pointLights);

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

    public void ChangeCubesMovings(int i, bool state) {
      ChangeMovingActions(i, state, ref _moveVolume);
    }
    
    public void ChangeLightsMovings(int i, bool state) {
      ChangeMovingActions(i, state, ref _moveFlashLight);
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

    public void ChangeDirLightDirection(Vector3 val) {
      for (int i = 0; i < _directionalLights.Count; ++i) {
        ((DirectionalLightColor)_directionalLights[i].ItsColor).Direction = val;
      }
    }

    public void ChangeDirLightColor(Vector3 val) {
      for (int i = 0; i < _directionalLights.Count; ++i) {
        ((DirectionalLightColor)_directionalLights[i].ItsColor).Ambient = val;
      }
    }

    public void ChangeDirLightDiffuse(Vector3 val) {
      for (int i = 0; i < _directionalLights.Count; ++i) {
        ((DirectionalLightColor)_directionalLights[i].ItsColor).Diffuse = val;
      }
    }

    public void ChangeDirLightSpecular(Vector3 val) {
      for (int i = 0; i < _directionalLights.Count; ++i) {
        ((DirectionalLightColor)_directionalLights[i].ItsColor).Specular = val;
      }
    }

    public void ChangePointLightColor(Vector3 val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Ambient = val;
      }
    }

    public void ChangePointLightDiffuse(Vector3 val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Diffuse = val;
      }
    }

    public void ChangePointLightSpecular(Vector3 val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Specular = val;
      }
    }

    public void ChangePointLightConstant(float val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Constant = val;
      }
    }

    public void ChangePointLightLinear(float val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Linear = val;
      }
    }

    public void ChangePointLightQuadratic(float val) {
      for (int i = 0; i < _pointLights.Count; ++i) {
        ((PointLightColor)_pointLights[i].ItsColor).Quadratic = val;
      }
    }

    public void ChangeFlashLightDirection(Vector3 val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Direction = val;
      }
    }

    public void ChangeFlashLightColor(Vector3 val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Ambient = val;
      }
    }


    public void ChangeFlashLightDiffuse(Vector3 val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Diffuse = val;
      }
    }

    public void ChangeFlashLightSpecular(Vector3 val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Specular = val;
      }
    }


    public void ChangeFlashLightConstant(float val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Constant = val;
      }
    }

    public void ChangeFlashLightLinear(float val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Linear = val;
      }
    }

    public void ChangeFlashLightQuadratic(float val) {
      for (int i = 0; i < _lights.Count; ++i) {
        ((FlashLightColor)_lights[i].ItsColor).Quadratic = val;
      }
    }



    //  //////////////////////////////////////////////////////////////////////////////////////
    public void ReplaceVolumes(int countOfSide, float step) {
      _volumes.Clear();
      _volumes.AddRange(Generator.GenerateVolumes(countOfSide, step));
      CreateAndBindVolumesBuffers();
    }


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
      int bias = 0;
      int step = length / 3 / 6;

      for (int g = 0; g < 6; ++g) {
        GL.DrawArrays(PrimitiveType.LineStrip, bias, step);
        bias += step;
      }
    }

    private void ShowPoints(int length) {
      GL.DrawArrays(PrimitiveType.Points, 0, length / 3);
    }

    private void MoveVolumes() {
      for (int i = 0; i < _volumes.Count; ++i) {
        var form = _volumes[i].ItsPosition;
        _moveVolume?.Invoke(ref form);
        _volumes[i].ItsPosition = form;
      }
    }

    private void MoveLights() {
      for (int i = 0; i < _lights.Count; ++i) {
        var light = _lights[i].ItsVolume.ItsPosition;
        _moveFlashLight?.Invoke(ref light);
        _lights[i].ItsVolume.ItsPosition = light;
      }
    }

    private void ShowVolumes() {
      ShaderAdjuster.AdjustShader(_interpolationKoeff, ref _shader, ShaderAdjuster.mode.morphingFactor);
      ShaderAdjuster.AdjustShader(ref _camera, ref _shader);
      AdjustShaderWithLights(ref _lights, ref _shader);
      AdjustShaderWithLights(ref _directionalLights, ref _shader);
      AdjustShaderWithLights(ref _pointLights, ref _shader);

      for (int i = 0; i < _volumes.Count; ++i) {
        ShaderAdjuster.AdjustShader(_volumes[i].ItsMaterial, ref _shader, 0);
        var modelMatrix = _volumes[i].ComputeModelMatrix();
        ShaderAdjuster.AdjustShader(ref modelMatrix, ref _shader, ShaderAdjuster.mode.modelmatrix);
        GL.BindVertexArray(_volumes[i].Vao);
        _showType(_volumes[i].ItsForm.Vertices.Length);
      }
    }

    private void AdjustShaderWithLights(ref List<Light> lights, ref Shader shader) {
      for (int i = 0; i < lights.Count; ++i) {
        ShaderAdjuster.AdjustShader(lights[i].ItsColor, ref shader, i);
        ShaderAdjuster.AdjustShader(lights[i].ItsVolume.ItsPosition.PosVr, ref _shader, i,
            ShaderAdjuster.mode.position);
      }
    }

    private void ShowLamps(List<Light> lights) {
      ShaderAdjuster.AdjustShader(ref _camera, ref _lampShader);

      for (int i = 0; i < lights.Count; ++i) {
        Matrix4 modelLamp = lights[i].ItsVolume.ComputeModelMatrix();
        ShaderAdjuster.AdjustShader(ref modelLamp, ref _lampShader, ShaderAdjuster.mode.modelmatrix);
        ShaderAdjuster.AdjustShader(lights[i].ItsVolume.ItsMaterial.Ambient, ref _lampShader, -1, ShaderAdjuster.mode.color);
        GL.BindVertexArray(lights[i].ItsVolume.Vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, lights[i].ItsVolume.ItsForm.Vertices.Length / 3);
      }
    }

    private void CreateAndBindVolumesBuffers() {
      for (int i = 0; i<_volumes.Count; ++i) {
        _volumes[i].Bind(ref _shader);
      }
    }

    private void CreateAndBindLampsBuffers() {
      for (int i = 0; i < _lights.Count; ++i) {
        _lights[i].Bind(ref _lampShader);
      }
      for (int i = 0; i < _pointLights.Count; ++i) {
        _pointLights[i].Bind(ref _lampShader);
      }
      for (int i = 0; i < _directionalLights.Count; ++i) {
        _directionalLights[i].Bind(ref _lampShader);
      }
    }

    private void ChangeMovingActions(int i, bool state, ref MoveVolume movingDelegate) {
      if (i == 0) {
        if (state) {
          movingDelegate += _circleShiftingMover.Move;
        } else {
          movingDelegate -= _circleShiftingMover.Move;
        }
      }

      if (i == 1) {
        if (state) {
          movingDelegate += _rotateaterMover.Move;
        } else {
          movingDelegate -= _rotateaterMover.Move;
        }
      }

      if (i == 2) {
        if (state) {
          movingDelegate += _upDownMover.Move;
        } else {
          movingDelegate -= _upDownMover.Move;
        }
      }
    }

  }
}



