
namespace SimpleDrawing.Model {
  public class CircleShiftingMover : IMover {

    public double Radius { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public int Speed { get; set; }

    private int _step;

    private double _deltaRads;

    public void Move(ref Volume source) {
      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      double radius = Math.Sqrt(Math.Pow(buff.X - CenterX, 2) + Math.Pow(buff.Y - CenterY, 2));
      buff.X = (float)(CenterX + radius * Math.Cos(_deltaRads * _step));
      buff.Y = (float)(CenterY + radius * Math.Sin(_deltaRads * _step));

      source.PosVr = buff;
      _step = (_step > 100 || _step < 0) ? _step - 1 : _step + 1;
    }

    public CircleShiftingMover(int speed, float xCenter, float yCenter) {
      Speed = speed;
      CenterX = xCenter;
      CenterY = yCenter;
      _deltaRads = Math.PI * 2 / (double) speed;
      _step = 0;
    }



  }
}
