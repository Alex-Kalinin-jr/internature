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
      _bottomColor = new Vector3(bottom[0] / 255.0f, bottom[1] / 255.0f, bottom[2] / 255.0f);
      _topColor = new Vector3(top[0] / 255.0f, top[1] / 255.0f, top[2] / 255.0f);
    }

    public static (Vector3, Vector3) GetColors() {
      return (_bottomColor, _topColor);
    }
  }
}
