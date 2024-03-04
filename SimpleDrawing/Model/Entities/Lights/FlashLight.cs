using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class FlashLight : DirectionalLight {

    public FlashLight() : base() {
      ItsColor = new FlashLightColor();
      ItsVolume.ItsPosition.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f);
      ((FlashLightColor)ItsColor).Direction = new Vector3(0.0f, 1.0f, 0.0f);
    }

  }
}

