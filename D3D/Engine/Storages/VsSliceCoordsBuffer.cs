using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct VsSliceCoordsBuffer {
    public int Xcoord;
    public int Ycoord; 
    public int Zcoord;

    public VsSliceCoordsBuffer(int x = -1, int y = -1, int z = -1) {
      Xcoord = x;
      Ycoord = y;
      Zcoord = z;
    }
  }

}