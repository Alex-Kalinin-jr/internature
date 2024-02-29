namespace SimpleDrawing.Model {
  public interface IPositioned {
    public OpenTK.Mathematics.Vector3 ScaleVr { get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; }
  }
}
