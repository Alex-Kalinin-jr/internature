
using SimpleDrawing.Model.Entities.ShaderAdjusters;

namespace SimpleDrawing.Model {
  public class FlashLight : DirectionalLight {

    public FlashLight() : base() {
      ItsColor = new FlashLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {
      shader.SetUniform3($"flashLights[{i}].position", ItsVolume.ItsPosition.PosVr);
      ColorAdjuster.AdjustShader((FlashLightColor)ItsColor, ref shader, i);
    }

  }
}

