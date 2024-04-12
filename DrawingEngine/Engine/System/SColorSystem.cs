using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace D3D {
  public class ColorSystem : DrawSystem {

    private static Vector3 _bottomColor = new Vector3(0.0f, 0.0f, 0.0f);
    private static Vector3 _topColor = new Vector3(1.0f, 1.0f, 1.0f);

    public static void ChangeColors(float[] bottom, float[] top) {
      var divider = 255.0f;
      _bottomColor = new Vector3(bottom[0] / divider, bottom[1] / divider, bottom[2] / divider);
      _topColor = new Vector3(top[0] / divider, top[1] / divider, top[2] / divider);
    }

    public static (Vector3, Vector3) GetColors() {
      return (_bottomColor, _topColor);
    }
  }
}
