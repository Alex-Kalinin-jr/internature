using OpenTK.Graphics.OpenGL4;

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

    private Shader _shader;
    private Shader _lampShader;
    private Shader _lettersShader;
    private Shader _edgesShader;

    private Show _showType;
    private MoveVolume _moveVolume;
    private MoveVolume _moveFlashLight;

    private string _renderTime;
    private float _interpolationKoeff;
    private bool _increase;

    private OpenTK.Mathematics.Vector3 _edgesColor;
    private OpenTK.Mathematics.Vector3 _pointsColor;

    CircleShiftingMover _circleShiftingMover;
    RotaterMover _rotateaterMover;
    UpDownMover _upDownMover;


    //  //////////////////////////////////////////////////////////////////////////////////////



    //  //////////////////////////////////////////////////////////////////////////////////////
    public SceneDrawer() {
      _width = 1024;
      _height = 768;

      _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _lampShader = new Shader("Shaders/lightShader.vert", "Shaders/lightShader.frag");
      _lettersShader = new Shader("Shaders/LetterShader.vert", "Shaders/LetterShader.frag");
      _edgesShader = new Shader("Shaders/shader.vert", "Shaders/lightShader.frag");

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

      _volumes.AddRange(Generator.GenerateVolumes(10, 2.0f));
      _lights.AddRange(Generator.GenerateLights(3, 2.0f));
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

    public void ChangeCubesMovings(int i, bool state) {
      ChangeMovingActions(i, state, ref _moveVolume);
    }
    
    public void ChangeLightsMovings(int i, bool state) {
      ChangeMovingActions(i, state, ref _moveFlashLight);
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
      val.ItsVolume.Vao = _lights[0].ItsVolume.Vao;
      _lights[0] = val;
    }

    public void ChangePointLight(PointLight val) {
      val.ItsVolume.Vao = _lights[1].ItsVolume.Vao;
      _lights[1] = val;
    }

    public void ChangeFlashLight(FlashLight val) {
      val.ItsVolume.Vao = _lights[2].ItsVolume.Vao;
      _lights[2] = val;
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
      _edgesShader.SetUniform3("Color", _edgesColor);

      int bias = 0;
      int step = length / 3 / 6;

      for (int g = 0; g < 6; ++g) {
        GL.DrawArrays(PrimitiveType.LineStrip, bias, step);
        bias += step;
      }
    }

    private void ShowPoints(int length) {
      _edgesShader.SetUniform3("Color", _pointsColor);
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
      for (int i = 2; i < _lights.Count; ++i) {
        var light = _lights[i].ItsVolume.ItsPosition;
        _moveFlashLight?.Invoke(ref light);
        _lights[i].ItsVolume.ItsPosition = light;
      }
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
        _showType(_volumes[i].ItsForm.Vertices.Length);
      }
    }

    private void ShowLamps() {

      _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      for (int i = 0; i < _lights.Count; ++i) {
        OpenTK.Mathematics.Matrix4 modelLamp = _lights[i].ItsVolume.ComputeModelMatrix();
        _lampShader.SetMatrix4("model", modelLamp);
        _lampShader.SetUniform3("Color", _lights[i].ItsVolume.ItsMaterial.Ambient);
        GL.BindVertexArray(_lights[i].ItsVolume.Vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _lights[i].ItsVolume.ItsForm.Vertices.Length / 3);
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



