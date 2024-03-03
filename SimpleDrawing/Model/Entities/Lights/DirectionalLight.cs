 
namespace SimpleDrawing.Model {
  public class DirectionalLight : Light {

    public DirectionalLight() {
      ItsVolume = new Cube(4);
      ItsVolume.ItsPosition.ScaleVr = new OpenTK.Mathematics.Vector3(0.1f, 0.1f, 0.1f);
      ItsColor = new DirectionalLightColor();
    }
  }
}

