using SharpDX;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex {
  public Vector3 Position { get; set; }
  public Vector2 Tex { get; set; }
  public Vector3 Normal { get; set; }

  public Vertex(Vector3 pos, Vector3 norm, Vector2 uv) {
    Position = pos;
    Tex = uv;
    Normal = norm;
  }
}