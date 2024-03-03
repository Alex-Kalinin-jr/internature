
namespace SimpleDrawing.Model {
  public class PointLightColor : DirectionalLightColor {
    public float Constant;
    public float Linear;
    public float Quadratic;

    public PointLightColor() : base() {
      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
    }
  }

}
