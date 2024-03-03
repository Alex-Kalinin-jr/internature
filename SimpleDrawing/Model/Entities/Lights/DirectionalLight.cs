
namespace SimpleDrawing.Model {
  public class DirectionalLight : Light {

    public DirectionalLight() {
      ItsVolume = new Cube(4);
      ItsColor = new DirectionalLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {
      if (ItsColor is DirectionalLightColor dL) {
        shader.SetUniform3($"dirlights[{i}].direction", dL.Direction);
      }
      shader.SetUniform3($"dirlights[{i}].color", ItsColor.Ambient);
      shader.SetUniform3($"dirlights[{i}].diffuse", ItsColor.Diffuse);
      shader.SetUniform3($"dirlights[{i}].specular", ItsColor.Specular);
    }

  }
}

