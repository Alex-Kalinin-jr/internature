using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class DirectionalLightColor : Color {

    public Vector3 Direction;

    public DirectionalLightColor() : base() {
// just an example direction
      Direction = new Vector3(-0.2f, -1.0f, -0.3f);
    }
  }

}
