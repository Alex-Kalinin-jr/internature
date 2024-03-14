
namespace SimpleDrawing.Model {
  public class UpDownMover : IMoving {
    public void Move(ref Position source) {

      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      if (buff.X >= 0) {
        buff.Y += 0.02f; // just an example step
      } else {
        buff.Y -= 0.02f;
      }
      
      source.PosVr = buff;
    }
  }
}
