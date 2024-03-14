
namespace SimpleDrawing.Model {
  public class ShiftingMover : IMoving {
    public OpenTK.Mathematics.Vector3 ScaleVr { get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; } 

    public void Move(ref Position source) {
      source.PosVr += PosVr;
      source.RotationVr += RotationVr;
      source.ScaleVr += ScaleVr;
    }

    public ShiftingMover(Position source) {
      ScaleVr = source.ScaleVr;
      PosVr = source.PosVr;
      RotationVr = source.RotationVr;
    }
  }
}
