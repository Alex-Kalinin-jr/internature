﻿
namespace SimpleDrawing.Model {
  public class FlashLightColor : PointLightColor {
    public float CutOff;
    public float OuterCutOff;
    public FlashLightColor() :base() {
      CutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(12.5f)); // just an example angles fo internal and external diffusing areas
      OuterCutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(20.5f));
    }
  }


}
