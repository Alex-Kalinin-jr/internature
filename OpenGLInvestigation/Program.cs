using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


class Program {
  public static void Main(string[] args) {

    var nativeWindowSettings = new NativeWindowSettings() {
      ClientSize = new Vector2i(800, 600),
      Title = "tratata",
      Flags = ContextFlags.ForwardCompatible,
    };

    using (OpenGLInvestigation.Window window = new OpenGLInvestigation.Window(GameWindowSettings.Default, nativeWindowSettings)) {
      window.Run();
    }
  }

}

