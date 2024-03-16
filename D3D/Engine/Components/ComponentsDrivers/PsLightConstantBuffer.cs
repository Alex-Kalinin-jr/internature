using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct PsLightConstantBuffer {
    public Vector4 Color;

    public Vector3 Position;

    public float AmbientStrength;

    public float SpecularStrength;

    public Vector3 ViewPos;

    public PsLightConstantBuffer(Vector4 D, Vector3 L) {
      Color = D;
      Position = L;
      AmbientStrength = 0.4f;
      SpecularStrength = 0.5f;
      ViewPos = new Vector3(0, 0, 0);
    }
  }
}
