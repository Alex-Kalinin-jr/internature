using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct Vertex {
    public Vector3 Position;
    public Color4 Color;

    public Vertex(Vector3 position, Color4 color) {
      Position = position;
      Color = color;
    }
  }
}
