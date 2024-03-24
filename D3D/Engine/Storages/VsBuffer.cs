using SharpDX;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct VsBuffer {
  public Vector3 Position;
  public Vector2 Tex;
  public Vector3 Normal;
  public Vector3 Color;
  public int[] Indices;

  public VsBuffer(Vector3 pos = default, 
                  Vector3 norm = default, 
                  Vector2 uv = default,
                  Vector3 color = default,
                  int[] indices = default) {
    Position = pos;
    Tex = uv;
    Normal = norm;
    Color = color;
    Indices = indices;
  }
}