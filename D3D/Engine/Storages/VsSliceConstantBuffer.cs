using System.Runtime.InteropServices;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct VsSliceConstantBuffer {
    public int Xcoord;
    public int Ycoord;
    public int Zcoord;
    public int Bias;

    public VsSliceConstantBuffer(int x = -1, int y = -1, int z = -1) {
      Xcoord = x;
      Ycoord = y;
      Zcoord = z;
      Bias = 0;
    }
  }

}