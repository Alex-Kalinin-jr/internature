
using System.Reflection.Metadata;

namespace SimpleDrawing.Model {
  public class PointLight : Light {

    public PointLight() : base() {
      ItsColor = new PointLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {
      shader.SetUniform3($"pointLights[{i}].position", ItsVolume.ItsPosition.PosVr);
      shader.SetUniform3($"pointLights[{i}].color",ItsColor.Ambient);
      shader.SetUniform3($"pointLights[{i}].diffuse", ItsColor.Diffuse);
      shader.SetUniform3($"pointLights[{i}].specular", ItsColor.Specular);

      if (ItsColor is PointLightColor pL) {
        shader.SetFloat($"pointLights[{i}].constant", pL.Constant);
        shader.SetFloat($"pointLights[{i}].linear", pL.Linear);
        shader.SetFloat($"pointLights[{i}].quadratic", pL.Quadratic);
      }
    }
  }

}