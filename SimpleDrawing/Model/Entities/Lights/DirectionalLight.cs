
using SimpleDrawing.Model.Entities.ShaderAdjusters;

namespace SimpleDrawing.Model {
  public class DirectionalLight : Light {

    public DirectionalLight() {
      ItsVolume = new Cube(4);
      ItsColor = new DirectionalLightColor();
    }

    public override void AdjustShader(ref Shader shader, int i) {
        ColorAdjuster.AdjustShader((DirectionalLightColor)ItsColor, ref shader, i);
    }

  }
}

