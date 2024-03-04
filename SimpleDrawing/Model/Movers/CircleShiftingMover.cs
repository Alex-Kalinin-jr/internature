
namespace SimpleDrawing.Model {
  public class CircleShiftingMover : IMoving {

    public double Radius { get; set; }

    private double _deltaRads;

    public CircleShiftingMover(int period) {
      _deltaRads = Math.PI * 2 / (double)period;
    }
    public void Move(ref Position source) {
      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      double radius = Math.Sqrt(Math.Pow(buff.X, 2) + Math.Pow(buff.Z, 2));
      var angle = Math.Atan2((double)buff.Z, (double)buff.X);
      buff.X = (float)(radius * Math.Cos(angle + _deltaRads));
      buff.Z = (float)(radius * Math.Sin(angle + _deltaRads));

      source.PosVr = buff;
    }
  }
}
