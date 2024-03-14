using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class FlashLight : DirectionalLight {

    public FlashLight() : base() {
      ItsColor = new FlashLightColor();
// as our scene consists of cubes with size 2x2x2, let us create light with different size.
      ItsVolume.ItsPosition.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f); // ScaleVr - good thing to do that.
// as we set our lights above our figures, let us define direction as vertical light (y - component)
      ((FlashLightColor)ItsColor).Direction = new Vector3(0.0f, 1.0f, 0.0f);
    }

  }
}

