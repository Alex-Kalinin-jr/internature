using SharpDX;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct VsBuffer {
  public Vector3 Position;
  public Vector2 Tex;
  public Vector3 Normal;
  public Vector3 Color;
  public int XGridCoord; 
  public int YGridCoord; 
  public int ZGridCoord; 

  public VsBuffer(Vector3 pos = default, 
                  Vector3 norm = default, 
                  Vector2 uv = default,
                  Vector3 color = default,
                  int x = -1, int y = -1, int z = -1) {
    Position = pos;
    Tex = uv;
    Normal = norm;
    Color = color;
    XGridCoord = x;
    YGridCoord = y;
    ZGridCoord = z;
  }
}