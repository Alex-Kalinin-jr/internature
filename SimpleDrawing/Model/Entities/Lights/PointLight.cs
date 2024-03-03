
using SimpleDrawing.Model.Entities.ShaderAdjusters;
using System.Reflection.Metadata;

namespace SimpleDrawing.Model {
  public class PointLight : Light {

    public PointLight() : base() {
      ItsColor = new PointLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {
      shader.SetUniform3($"pointLights[{i}].position", ItsVolume.ItsPosition.PosVr);
      ColorAdjuster.AdjustShader((PointLightColor)ItsColor, ref shader, i);
    }
  }

}