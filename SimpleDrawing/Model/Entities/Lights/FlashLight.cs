namespace SimpleDrawing.Model {
  public class FlashLight : DirectionalLight {

    public FlashLight() : base() {
      ItsColor = new FlashLightColor();
    }

  }
}

