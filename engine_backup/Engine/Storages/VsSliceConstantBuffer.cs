using System.Runtime.InteropServices;

namespace D3D {
  /// <summary>
  /// Struct for storing slice constant buffer data used in DirectX applications.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct VsSliceConstantBuffer {
    /// <summary>
    /// X coordinate.
    /// </summary>
    public int Xcoord;

    /// <summary>
    /// Y coordinate.
    /// </summary>
    public int Ycoord;

    /// <summary>
    /// Z coordinate.
    /// </summary>
    public int Zcoord;

    /// <summary>
    /// Bias value. At now is used to check what is drawn - net or volumes ant to set apporpriate color
    /// </summary>
    public int Bias;

    /// <summary>
    /// Initializes a new instance of the <see cref="VsSliceConstantBuffer"/> struct with optional coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    public VsSliceConstantBuffer(int x = -1, int y = -1, int z = -1) {
      Xcoord = x;
      Ycoord = y;
      Zcoord = z;
      Bias = -1;
    }
  }
}
