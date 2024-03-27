using SharpDX;
using System.Runtime.InteropServices;

/// <summary>
/// Struct for storing vertex buffer data used in DirectX applications.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct VsBuffer {
  /// <summary>
  /// Position of the vertex in 3D space.
  /// </summary>
  public Vector3 Position;

  /// <summary>
  /// Texture coordinates of the vertex.
  /// </summary>
  public Vector2 Tex;

  /// <summary>
  /// Normal vector of the vertex.
  /// </summary>
  public Vector3 Normal;

  /// <summary>
  /// Color of the vertex.
  /// </summary>
  public Vector3 Color;

  /// <summary>
  /// X-coordinate of the grid to which the vertex belongs.
  /// </summary>
  public int XGridCoord;

  /// <summary>
  /// Y-coordinate of the grid to which the vertex belongs.
  /// </summary>
  public int YGridCoord;

  /// <summary>
  /// Z-coordinate of the grid to which the vertex belongs.
  /// </summary>
  public int ZGridCoord;

  /// <summary>
  /// Initializes a new instance of the <see cref="VsBuffer"/> struct.
  /// </summary>
  /// <param name="pos">Position of the vertex.</param>
  /// <param name="norm">Normal vector of the vertex.</param>
  /// <param name="uv">Texture coordinates of the vertex.</param>
  /// <param name="color">Color of the vertex.</param>
  /// <param name="x">X-coordinate of the grid to which the vertex belongs.</param>
  /// <param name="y">Y-coordinate of the grid to which the vertex belongs.</param>
  /// <param name="z">Z-coordinate of the grid to which the vertex belongs.</param>
  public VsBuffer(Vector3 pos = default,
                  Vector3 norm = default,
                  Vector2 uv = default,
                  Vector3 color = default,
                  int x = -1,
                  int y = -1,
                  int z = -1) {
    Position = pos;
    Tex = uv;
    Normal = norm;
    Color = color;
    XGridCoord = x;
    YGridCoord = y;
    ZGridCoord = z;
  }
}
