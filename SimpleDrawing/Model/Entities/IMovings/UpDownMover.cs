
namespace SimpleDrawing.Model {
  public class UpDownMover : IMoving {
    public void Move(ref Position source) {

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
