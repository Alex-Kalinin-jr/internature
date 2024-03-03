
namespace SimpleDrawing.Model {
  public class DirectionalLight : Light {

    public DirectionalLight() {
      ItsVolume = new Cube(4);
      ItsColor = new DirectionalLightColor();
    }
  }
}

