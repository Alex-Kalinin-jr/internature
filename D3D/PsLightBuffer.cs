using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace D3D {
  [StructLayout(LayoutKind.Sequential)]
  public struct LightBuffer {
    public Vector4 Diffuse;

    public Vector3 LightDirection;

    public float Padding;

    public LightBuffer(Vector4 D, Vector3 L) {
      Diffuse = D;
      LightDirection = L;
      Padding = 0;
    }

  }
}
