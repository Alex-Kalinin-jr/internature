﻿using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleDrawing;

public class Window : GameWindow {

  public struct TmpDirLight {
    public System.Numerics.Vector3 Direction;
    public System.Numerics.Vector3 Color;
    public System.Numerics.Vector3 Diffuse;
    public System.Numerics.Vector3 Specular;
  };
  private TmpDirLight _directionalLight;

  public struct TmpPointLight {
    public System.Numerics.Vector3 Direction;
    public System.Numerics.Vector3 Color;
    public System.Numerics.Vector3 Diffuse;
    public System.Numerics.Vector3 Specular;
    public float Constant;
    public float Linear;
    public float Quadratic;
  }
  private TmpPointLight _pointLight;

  public struct TmpFlashLight {
    public System.Numerics.Vector3 Position;
    public System.Numerics.Vector3 Direction;
    public System.Numerics.Vector3 Color;
    public System.Numerics.Vector3 Diffuse;
    public System.Numerics.Vector3 Specular;
    public float Constant;
    public float Linear;
    public float Quadratic;
    public float CutOff;
    public float OuterCutOff;
  }
  private TmpFlashLight _flashLight;

  private bool _areFacesDrawn;
  private bool _areEdgesDrawn;
  private bool _arePointsDrawn;

  private bool _areRotatedOne;
  private bool _areRotatedTwo;
  private bool _areShiftedUpDown;

  private bool _areLightsRotatedOne;
  private bool _areLightsRotatedTwo;
  private bool _areLightsRotatedThree;

  int _cubesCount;
  float _step;

  private System.Numerics.Vector3 _edgesColor;
  private System.Numerics.Vector3 _pointsColor;



  private static DebugProc _debugProcCallback = DebugCallback;
  private static GCHandle _debugProcCallbackHandle;
  private Stopwatch _stopWatch;

  ImGuiController _controller;
  Model.SceneRender _scene;

  Vector3 ConvertVector(System.Numerics.Vector3 v) {
    return new Vector3(v.X, v.Y, v.Z);
  }

  public Window() : base(GameWindowSettings.Default, new NativeWindowSettings() {
    Size = new Vector2i(1024, 768), APIVersion = new Version(3, 3)
  }) {

    _directionalLight = new TmpDirLight();
    _pointLight = new TmpPointLight();
    _flashLight = new TmpFlashLight();

    _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
    _scene = new Model.SceneRender(this);

    _areFacesDrawn = true;
    _areEdgesDrawn = false;
    _arePointsDrawn = false;

    _areRotatedOne = false;
    _areShiftedUpDown = false;
    _areRotatedTwo = false;

    _edgesColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
    _pointsColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);

    _stopWatch = Stopwatch.StartNew();
    _cubesCount = 10;
    _step = 2.0f;
  }


  private static void DebugCallback(DebugSource source, DebugType type, int id,
  DebugSeverity severity, int length, IntPtr message, IntPtr userParam) {
    string messageString = Marshal.PtrToStringAnsi(message, length);
    // there try to insert into app
    Console.WriteLine($"{severity} {type} | {messageString}");

    if (type == DebugType.DebugTypeError)
      throw new Exception(messageString);
  }

  void SetupDebugging() {
    _debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);
    GL.DebugMessageCallback(_debugProcCallback, IntPtr.Zero);
    GL.Enable(EnableCap.DebugOutput);
    GL.Enable(EnableCap.DebugOutputSynchronous);
  }

  protected override void OnLoad() {
    base.OnLoad();
    SetupDebugging();
    Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
    VSync = VSyncMode.On;

    Error.Check();
  }

  protected override void OnResize(ResizeEventArgs e) {
    base.OnResize(e);
    GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    _controller.WindowResized(ClientSize.X, ClientSize.Y);
  }

  protected override void OnRenderFrame(FrameEventArgs e) {
    base.OnRenderFrame(e);

    _controller.Update(this, (float)e.Time);

    GL.ClearColor(1.0f, 1.0f, 0.0f, 0.0f);
    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

    _controller.StartDockspace();
    // customization
    CreateShowingTypeButtons();
    // CreateColorPalette();
    CreatePointLightPalette();
    CreateFlasLightPalette();
    CreateDirLightPalette();
    CreateVolumesChanger();
    CreateMoveVolumeCheckboxes();

    Error.Check();
    _scene.DrawViewportWindow();

    Error.Check();
    _controller.EndDockspace();

    _controller.Render();

    ImGuiController.CheckGLError("End of frame");

    SwapBuffersWithTimeMeasurement();
  }

  protected void SwapBuffersWithTimeMeasurement() {

    _stopWatch.Stop();
    float millisec = (float)_stopWatch.ElapsedMilliseconds / 1000.0f;
    float fps = 1.0f / millisec;
    string strFps = fps > 60 ? "60" : ((int)fps).ToString();

    _scene.SetTime(strFps);
    SwapBuffers();
    _stopWatch = Stopwatch.StartNew();

  }



  protected override void OnTextInput(TextInputEventArgs e) {
    base.OnTextInput(e);

    _controller.PressChar((char)e.Unicode);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e) {
    base.OnMouseWheel(e);

    _controller.MouseScroll(e.Offset);
  }

  protected override void OnUnload() {
    _scene.Dispose();
    _controller.Dispose();

    base.OnUnload();
  }


  private void CreateShowingTypeButtons() {

    ImGui.Begin("Drawing Mode");
    if (ImGui.Checkbox("Solid", ref _areFacesDrawn)) {
      if (!_areEdgesDrawn && !_arePointsDrawn) {
        _areFacesDrawn = true;
      } else {
        _scene.ChangeShowingType(0, _areFacesDrawn);
      }
    }

    if (ImGui.Checkbox("Frame", ref _areEdgesDrawn)) {
      if (!_areFacesDrawn && !_arePointsDrawn) {
        _areEdgesDrawn = true;
      } else {
        _scene.ChangeShowingType(1, _areEdgesDrawn);
      }
    }

    if (ImGui.Checkbox("Points", ref _arePointsDrawn)) {
      if (!_areEdgesDrawn && !_areFacesDrawn) {
        _arePointsDrawn = true;
      } else {
        _scene.ChangeShowingType(2, _arePointsDrawn);
      }
    }
    ImGui.End();

  }

  private void CreateMoveVolumeCheckboxes() {

    ImGui.Begin("Volumes");
    if (ImGui.Checkbox("Rotate 1", ref _areRotatedOne)) {
      _scene.ChangeMovements(0, _areRotatedOne);
    }

    if (ImGui.Checkbox("Rotate 2", ref _areRotatedTwo)) {
      _scene.ChangeMovements(1, _areRotatedTwo);
    }

    if (ImGui.Checkbox("Shift Up-Down", ref _areShiftedUpDown)) {
      _scene.ChangeMovements(2, _areShiftedUpDown);

    }
    ImGui.End();

    ImGui.Begin("Lights");
    if (ImGui.Checkbox("Rotate 1", ref _areLightsRotatedOne)) {
      _scene.ChangeLightsMovements(0, _areLightsRotatedOne);
    }

    if (ImGui.Checkbox("Rotate 2", ref _areLightsRotatedTwo)) {
      _scene.ChangeLightsMovements(1, _areLightsRotatedTwo);
    }

    if (ImGui.Checkbox("Shift Up-Down", ref _areLightsRotatedThree)) {
      _scene.ChangeLightsMovements(2, _areLightsRotatedThree);

    }
    ImGui.End();

  }

  private void CreateDirLightPalette() {
    ImGui.Begin("directional light");
    if (ImGui.SliderFloat3("direction", ref _directionalLight.Direction, -1.0f, 1.0f)) {
      _scene.ChangeDirLightDirection(ConvertVector(_directionalLight.Direction));
    }
    if (ImGui.SliderFloat3("color", ref _directionalLight.Color, 0.0f, 1.0f)) {
      _scene.ChangeDirLightColor(ConvertVector(_directionalLight.Color));
    }
    if (ImGui.SliderFloat3("diffuse", ref _directionalLight.Diffuse, 0.0f, 1.0f)) {
      _scene.ChangeDirLightDiffuse(ConvertVector(_directionalLight.Diffuse));
    }
    if (ImGui.SliderFloat3("specular", ref _directionalLight.Specular, 0.0f, 1.0f)) {
      _scene.ChangeDirLightSpecular(ConvertVector(_directionalLight.Specular));
    }
    ImGui.End();
  }

  private void CreatePointLightPalette() {
    ImGui.Begin("point light");
    if (ImGui.SliderFloat3("color", ref _pointLight.Color, 0.0f, 1.0f)) {
      _scene.ChangePointLightColor(ConvertVector(_pointLight.Color));
    }
    if (ImGui.SliderFloat3("diffuse", ref _pointLight.Diffuse, 0.0f, 1.0f)) {
      _scene.ChangePointLightDiffuse(ConvertVector(_pointLight.Diffuse));
    }
    if (ImGui.SliderFloat3("specular", ref _pointLight.Specular, 0.0f, 1.0f)) {
      _scene.ChangePointLightSpecular(ConvertVector(_pointLight.Specular));
    }
    if (ImGui.SliderFloat("constant koeff", ref _pointLight.Constant, 0.5f, 1.0f)) {
      _scene.ChangePointLightQuadratic(_pointLight.Constant);
    }
    if (ImGui.SliderFloat("linear koeff", ref _pointLight.Linear, 0.07f, 0.3f)) {
      _scene.ChangePointLightQuadratic(_pointLight.Linear);
    }
    if (ImGui.SliderFloat("quadratic koeff", ref _pointLight.Quadratic, 0.0f, 0.07f)) {
      _scene.ChangePointLightQuadratic(_pointLight.Quadratic);
    }
    ImGui.End();
  }


  private void CreateVolumesChanger() {
    ImGui.Begin("Volumes");
    if (ImGui.SliderInt("Count", ref _cubesCount, 1, 100) ||
        ImGui.SliderFloat("_step", ref _step, 2.0f, 10.0f)) {
      _scene.ReGenerateVolumes(_cubesCount, _step);
    }

    ImGui.End();
  }

  private void CreateFlasLightPalette() {
    ImGui.Begin("flash light");
    /*
    if (ImGui.SliderFloat3("position", ref _flashLight.Position, -6.0f, 6.0f)) {
      _scene.ChangeFlashLightPosition(ConvertVector(_flashLight.Position));
    }
    */
    if (ImGui.SliderFloat3("direction", ref _flashLight.Direction, -1.0f, 1.0f)) {
      _scene.ChangeFlashLightDirection(ConvertVector(_flashLight.Direction));
    }
    if (ImGui.SliderFloat3("color", ref _flashLight.Color, 0.0f, 1.0f)) {
      _scene.ChangeFlashLightColor(ConvertVector(_flashLight.Color));
    }
    if (ImGui.SliderFloat3("diffuse", ref _flashLight.Diffuse, 0.0f, 1.0f)) {
      _scene.ChangeFlashLightDiffuse(ConvertVector(_flashLight.Diffuse));
    }
    if (ImGui.SliderFloat3("specular", ref _flashLight.Specular, 0.0f, 1.0f)) {
      _scene.ChangeFlashLightSpecular(ConvertVector(_flashLight.Specular));
    }
    if (ImGui.SliderFloat("constant koeff", ref _flashLight.Constant, 0.5f, 1.0f)) {
      _scene.ChangeFlashLightConstant(_pointLight.Constant);
    }
    if (ImGui.SliderFloat("linear koeff", ref _flashLight.Linear, 0.07f, 0.3f)) {
      _scene.ChangeFlashLightLinear(_pointLight.Linear);
    }
    if (ImGui.SliderFloat("quadratic koeff", ref _flashLight.Quadratic, 0.0f, 0.07f)) {
      _scene.ChangeFlashLightQuadratic(_pointLight.Quadratic);
    }
    ImGui.End();
  }

  /*
  private void CreateColorPalette() {
    ImGui.Begin("edges");

    if (ImGui.ColorEdit3("", ref _edgesColor) || ImGui.ColorPicker3("", ref _edgesColor)) {
      _scene.SetEdgesColor(_edgesColor);
    }
    ImGui.End();

    ImGui.Begin("points");
    if (ImGui.ColorEdit3("", ref _pointsColor) || ImGui.ColorPicker3("", ref _pointsColor)) {
      _scene.SetPointsColor(_pointsColor);
    }
    ImGui.End();
  }
*/

  protected override void OnUpdateFrame(FrameEventArgs e) {
    base.OnUpdateFrame(e);

    if (!IsFocused) {
      return;
    }

    var input = KeyboardState;

    if (input.IsKeyDown(Keys.Escape)) {
      Close();
    }

    if (input.IsKeyDown(Keys.W)) {
      _scene.MoveCameraFwd((float)e.Time);
    }

    if (input.IsKeyDown(Keys.S)) {
      _scene.MoveCameraBack((float)e.Time);
    }

    if (input.IsKeyDown(Keys.A)) {
      _scene.MoveCameraRight((float)e.Time);
    }

    if (input.IsKeyDown(Keys.D)) {
      _scene.MoveCameraLeft((float)e.Time);
    }

    if (input.IsKeyDown(Keys.RightShift)) {
      _scene.MoveCameraDown((float)e.Time);
    }

    if (input.IsKeyDown(Keys.Space)) {
      _scene.MoveCameraUp((float)e.Time);
    }

    if (input.IsKeyDown(Keys.Up)) {
      _scene.ChangeCameraPitch(-1.0f);
    }

    if (input.IsKeyDown(Keys.Down)) {
      _scene.ChangeCameraPitch(1.0f);
    }

    if (input.IsKeyDown(Keys.Left)) {
      _scene.ChangeCameraYaw(-1.0f);
    }

    if (input.IsKeyDown(Keys.Right)) {
      _scene.ChangeCameraYaw(1.0f);
    }
  }

}




