
namespace SimpleDrawing.Model {

  public class RotationMover : IMoving {
    const float exampleInternalRad = 2.0f;
    const float exampleMiddleRad = 4.0f;
    const float exampleOuterRad = 6.0f;
    const float exampleInternalCondition = 3.0f;
    const float exampleExternalCondition = 6.0f;

    public void Move(ref Position source) {
      OpenTK.Mathematics.Vector3 buff = source.PosVr;
      OpenTK.Mathematics.Vector3 buffRot = source.RotationVr;
      double radius = Math.Sqrt(Math.Pow(buff.X, 2) + Math.Pow(buff.Z, 2));
      source.RotationVr = buffRot;

      if (radius > exampleExternalCondition) {
        buffRot.Y += exampleOuterRad;
      } else if (radius > exampleInternalCondition && radius < exampleExternalCondition) {
        buffRot.Y += exampleInternalRad;
      } else {
        buffRot.Y += exampleMiddleRad;
      }

      source.RotationVr = buffRot;
    }
  }
}
