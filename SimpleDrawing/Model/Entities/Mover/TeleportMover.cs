using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDrawing.Model {
  public class TeleportMover : IMover {
    public void Move(ref IPositioned source, IPositioned target) {
      source.PosVr = target.PosVr;
      source.RotationVr = target.RotationVr;
      source.ScaleVr = target.ScaleVr;
    }
  }
}
