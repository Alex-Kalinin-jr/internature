
namespace SimpleDrawing.Model {
  public class FlashLight : DirectionalLight {

    public FlashLight() : base() {
      ItsColor = new FlashLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {

      shader.SetUniform3($"flashLights[{i}].position", ItsVolume.ItsPosition.PosVr);

      if (ItsColor is DirectionalLightColor dL) {
        shader.SetUniform3($"flashLights[{i}].direction", dL.Direction);
        shader.SetUniform3($"flashLights[{i}].color", dL.Ambient);
        shader.SetUniform3($"flashLights[{i}].diffuse", dL.Diffuse);
        shader.SetUniform3($"flashLights[{i}].specular", dL.Specular);
      }

      if (ItsColor is FlashLightColor fL) {
        shader.SetFloat($"flashLights[{i}].cutOff", fL.CutOff);
        shader.SetFloat($"flashLights[{i}].outerCutOff", fL.OuterCutOff);
        shader.SetFloat($"flashLights[{i}].constant", fL.Constant);
        shader.SetFloat($"flashLights[{i}].linear", fL.Linear);
        shader.SetFloat($"flashLights[{i}].quadratic", fL.Quadratic);
      }
    }

  }
}

