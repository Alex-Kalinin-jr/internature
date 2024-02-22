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

  private System.Numerics.Vector3 _facesColor;
  private System.Numerics.Vector3 _edgesColor;
  private System.Numerics.Vector3 _pointsColor;

  private static DebugProc _debugProcCallback = DebugCallback;
  private static GCHandle _debugProcCallbackHandle;

  ImGuiController _controller;
  SceneRender _scene;
  Camera _camera;

  public Window() : base(GameWindowSettings.Default, new NativeWindowSettings() {
    Size = new Vector2i(1024, 768), APIVersion = new Version(3, 3)
  }) {
    _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
    _scene = new SceneRender(this);
    _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

    _areFacesDrawn = true;
    _areEdgesDrawn = false;
    _arePointsDrawn = false;

    _facesColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
    _edgesColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
    _pointsColor = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
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

  private void CreateColorPalette() {

    ImGui.Begin("faces");
    if (ImGui.ColorEdit3("", ref _facesColor) || ImGui.ColorPicker3("", ref _facesColor)) {
      _scene.SetFacesColor(_facesColor);
    }
    ImGui.End();

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

    const float cameraSpeed = 1.5f;

    if (input.IsKeyDown(Keys.W)) {
      _camera.Position += _camera.Front * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.S)) {
      _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.A)) {
      _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.D)) {
      _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.LeftShift)) {
      _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.Space)) {
      _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time;
    }

    if (input.IsKeyDown(Keys.Up)) {
      _camera.Pitch -= 1;
    }

    if (input.IsKeyDown(Keys.Down)) {
      _camera.Pitch += 1;
    }

    if (input.IsKeyDown(Keys.Left)) {
      _camera.Yaw -= 1;
    }

    if (input.IsKeyDown(Keys.Right)) {
      _camera.Yaw += 1;
    }

    PassMatrices();
  }

  public void PassMatrices() {
    _scene.SetCameraMatrices(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
  }

}




