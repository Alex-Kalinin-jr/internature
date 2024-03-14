
namespace SimpleDrawing.Model {
  public class PointLightColor : DirectionalLightColor {
    public float Constant;
    public float Linear;
    public float Quadratic;

    public PointLightColor() : base() {
      Constant = 1.0f; // just an examples
      Linear = 0.09f; // just an examples, but do not set more than 0.3
      Quadratic = 0.032f; // just an example, set significantly less that linear koeff
    }
  }

}
