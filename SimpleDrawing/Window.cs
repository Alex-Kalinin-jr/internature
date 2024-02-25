using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SimpleDrawing.Entities;
using System.Runtime.InteropServices;

namespace SimpleDrawing;

public class Window : GameWindow {



  private bool _areFacesDrawn;
  private bool _areEdgesDrawn;
  private bool _arePointsDrawn;

  private System.Numerics.Vector3 _edgesColor;
  private System.Numerics.Vector3 _pointsColor;

  private DirectionalLight _directionalLight;
  private PointLight _pointLight;
  private FlashLight _flashLight;
  private System.Numerics.Vector3 _flashPos;

  private static DebugProc _debugProcCallback = DebugCallback;
  private static GCHandle _debugProcCallbackHandle;

  ImGuiController _controller;
  SceneRender _scene;

  public Window() : base(GameWindowSettings.Default, new NativeWindowSettings() {
    Size = new Vector2i(1024, 768), APIVersion = new Version(3, 3)
  }) {
    _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
    _scene = new SceneRender(this);

    _areFacesDrawn = true;
    _areEdgesDrawn = false;
    _arePointsDrawn = false;

    _edgesColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
    _pointsColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);

    _directionalLight = new DirectionalLight();
    _pointLight = new PointLight();
    _flashLight = new FlashLight();
    _flashLight._form.PosVr = new Vector3(0.0f, 0.5f, 6.0f);
    _flashLight._direction = new System.Numerics.Vector3(0.0f, 0.0f, -1.0f);
    _flashLight._form.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f);
    _flashPos = new System.Numerics.Vector3(2.0f, -2.0f, 2.0f) ;
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
    CreateColorPalette();
    CreatePointLightPalette();
    CreateFlasLightPalette();
    CreateDirLightPalette();

    Error.Check();
    _scene.DrawViewportWindow();

    Error.Check();
    _controller.EndDockspace();

    _controller.Render();

    ImGuiController.CheckGLError("End of frame");

    SwapBuffers();
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

  private void CreateDirLightPalette() {
    ImGui.Begin("directional light");
    if (ImGui.SliderFloat3("direction", ref _directionalLight._direction, -1.0f, 1.0f) ||
        ImGui.SliderFloat3("color", ref _directionalLight._color, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("diffuse", ref _directionalLight._diffuse, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("specular", ref _directionalLight._specular, 0.0f, 1.0f)) {
      _scene.ChangeDirectionalLight(_directionalLight);
    }
    ImGui.End();
  }

  private void CreatePointLightPalette() {
    ImGui.Begin("point light");
    if (ImGui.SliderFloat3("color", ref _pointLight._color, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("diffuse", ref _pointLight._diffuse, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("specular", ref _pointLight._specular, 0.0f, 1.0f) ||
        ImGui.SliderFloat("constant koeff", ref _pointLight._constant, 0.5f, 1.0f) ||
        ImGui.SliderFloat("linear koeff", ref _pointLight._linear, 0.07f, 0.3f) ||
        ImGui.SliderFloat("quadratic koeff", ref _pointLight._quadratic, 0.0f, 0.07f)) {
      _scene.ChangePointLight(_pointLight);
    }
    ImGui.End();
  }

  private void CreateFlasLightPalette() {
    ImGui.Begin("flash light");

    if (ImGui.SliderFloat3("position", ref _flashPos, -6.0f, 6.0f)) {
      _flashLight._form.PosVr = new Vector3(_flashPos.X, _flashPos.Y, _flashPos.Z);
      _scene.ChangeFlashLight(_flashLight);
    }

    if (ImGui.SliderFloat3("direction", ref _flashLight._direction, -1.0f, 1.0f) ||
        ImGui.SliderFloat3("color", ref _flashLight._color, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("diffuse", ref _flashLight._diffuse, 0.0f, 1.0f) ||
        ImGui.SliderFloat3("specular", ref _flashLight._specular, 0.0f, 1.0f) ||
        ImGui.SliderFloat("constant koeff", ref _flashLight._constant, 0.5f, 1.0f) ||
        ImGui.SliderFloat("linear koeff", ref _flashLight._linear, 0.07f, 0.3f) ||
        ImGui.SliderFloat("quadratic koeff", ref _flashLight._quadratic, 0.0f, 0.07f)
        ) {
      _scene.ChangeFlashLight(_flashLight);
    }
    ImGui.End();
  }


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




