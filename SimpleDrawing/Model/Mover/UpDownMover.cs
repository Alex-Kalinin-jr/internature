
namespace SimpleDrawing.Model {
  internal class UpDownMover {
    public void Move(ref Volume source) {
      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      if (buff.X >= 0) {
        buff.Y += 0.005f;
      } else {
        buff.Y -= 0.005f;
      }
      
      source.PosVr = buff;
    }
  }
}
