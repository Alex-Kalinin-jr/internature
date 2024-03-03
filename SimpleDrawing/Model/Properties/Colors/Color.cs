using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class Color {
    public Vector3 Ambient;
    public Vector3 Diffuse;
    public Vector3 Specular;

    public Color() {
      Ambient = new Vector3(1.0f, 1.0f, 1.0f);
      Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
      Specular = new Vector3(0.5f, 0.5f, 0.5f);
    }
  }
}
