
namespace SimpleDrawing.Model {
  public class PointLight : Light {

    public PointLight() : base() {
      ItsColor = new PointLightColor();
    }
  }

}