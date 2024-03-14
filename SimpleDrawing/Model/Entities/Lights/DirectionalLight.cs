 
namespace SimpleDrawing.Model {
  public class DirectionalLight : Light {

    public DirectionalLight() {
      ItsVolume = new Cube(4);
// as our scene consists of cubes with size 2x2x2, let us create light with different size.
      ItsVolume.ItsPosition.ScaleVr = new OpenTK.Mathematics.Vector3(0.1f, 0.1f, 0.1f); // ScaleVr - good thing to do that.
      ItsColor = new DirectionalLightColor();
    }
  }
}

