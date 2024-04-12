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
      _bottomColor = new Vector3(bottom[0], bottom[1], bottom[2]);
      _topColor = new Vector3(top[0], top[1], top[2]);
    }

    public static (Vector3, Vector3) GetColors() {
      return (_topColor, _bottomColor);
    }
  }
}
