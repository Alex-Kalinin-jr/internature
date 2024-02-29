
namespace SimpleDrawing.Model {
  public class Position {
    public OpenTK.Mathematics.Vector3 ScaleVr { get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; }

    public Position() {
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }
  }
}
