
namespace SimpleDrawing.Model {
  public class ShiftingMover : IMover, IPositioned {
    public OpenTK.Mathematics.Vector3 ScaleVr { get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; } 

    public void Move(ref Volume source) {
      source.PosVr += PosVr;
      source.RotationVr += RotationVr;
      source.ScaleVr += ScaleVr;
    }

    public ShiftingMover(IPositioned source) {
      ScaleVr = source.ScaleVr;
      PosVr = source.PosVr;
      RotationVr = source.RotationVr;
    }
  }
}
