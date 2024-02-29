using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDrawing.Model {
  public class RotaterMover {
    public void Move(ref Volume source) {
      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      OpenTK.Mathematics.Vector3 buffRot = source.RotationVr;
      double radius = Math.Sqrt(Math.Pow(buff.X, 2) + Math.Pow(buff.Z, 2));
      source.RotationVr = buffRot;
      if (radius > 6) {
        buffRot.Y += 4.0f;
      } else if (radius > 3 && radius < 6) {
        buffRot.Y += 2.0f;
      } else {
        buffRot.Y += 0.5f;
      }

      source.RotationVr = buffRot;
    }
  }
}
