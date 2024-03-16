using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct VsMvpConstantBuffer {
    public Matrix world;
    public Matrix view;
    public Matrix projection;
    public Matrix modelMatrix;
  }
}
