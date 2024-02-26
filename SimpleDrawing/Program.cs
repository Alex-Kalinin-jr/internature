using OpenTK.Graphics.OpenGL4;
using SimpleDrawing.Entities;

namespace SimpleDrawing;

class Program {
  static void Main() {
    Window wnd = new Window();
    wnd.Run();
  }
}

static class Error {
  public static void Check() {
    ErrorCode errorCode = GL.GetError();
    if (errorCode != ErrorCode.NoError) {
      throw new InvalidOperationException();
    }
  }
}