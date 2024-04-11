using SharpDX.DirectInput;

namespace D3D {
  public class KeyboardSystem {
    private Keyboard _keyboard;

    public KeyboardSystem() {
      _keyboard = new Keyboard(new DirectInput());
      _keyboard.Acquire();
    }

    /// <summary>
    /// Performs actions regarding to keyboard inputs.
    /// </summary>
    public void ProcessInput() {
      _keyboard.Poll();
      var keyboardState = _keyboard.GetCurrentState();
      foreach (var key in keyboardState.PressedKeys) {
        switch (key) {
          case Key.W:
            CameraSystem.ShiftFwd();
            break;
          case Key.S:
            CameraSystem.ShiftBack();
            break;
          case Key.D:
            CameraSystem.ShiftRight();
            break;
          case Key.A:
            CameraSystem.ShiftLeft();
            break;
          case Key.Q:
            CameraSystem.ShiftUp();
            break;
          case Key.E:
            CameraSystem.ShiftDown();
            break;
        }
      }
    }
  }
}
