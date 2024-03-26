using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  /// <summary>
  /// Struct for storing vertex shader constant buffer data used in DirectX applications.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct VsMvpConstantBuffer {
    /// <summary>
    /// World transformation matrix.
    /// </summary>
    public Matrix world;

    /// <summary>
    /// View transformation matrix.
    /// </summary>
    public Matrix view;

    /// <summary>
    /// Projection transformation matrix.
    /// </summary>
    public Matrix projection;

    /// <summary>
    /// Model transformation matrix.
    /// </summary>
    public Matrix modelMatrix;
  }
}
