
namespace SimpleDrawing.Model {
  public class PointLight : Light {

    public System.Numerics.Vector3 Color;
    public System.Numerics.Vector3 Diffuse;
    public System.Numerics.Vector3 Specular;
    public float Constant;
    public float Linear;
    public float Quadratic;

    public PointLight() {
      ItsVolume = new Cube(4);
      Material material = new Material();
      material.Ambient = new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f);
      ItsVolume.ItsMaterial = material;
      ItsVolume.ItsPosition.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

      Color = new System.Numerics.Vector3(0.05f, 0.05f, 0.05f);
      Diffuse = new System.Numerics.Vector3(0.8f, 0.8f, 0.8f);
      Specular = new System.Numerics.Vector3(1.0f, 1.0f, 1.0f);

      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
    }

    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"pointLights[{i}].position", ItsVolume.ItsPosition.PosVr);
      shader.SetUniform3($"pointLights[{i}].color",
        new OpenTK.Mathematics.Vector3(Color.X, Color.Y, Color.Z));
      shader.SetUniform3($"pointLights[{i}].diffuse",
        new OpenTK.Mathematics.Vector3(Diffuse.X, Diffuse.Y, Diffuse.Z));
      shader.SetUniform3($"pointLights[{i}].specular",
        new OpenTK.Mathematics.Vector3(Specular.X, Specular.Y, Specular.Z));
      shader.SetFloat($"pointLights[{i}].constant", Constant);
      shader.SetFloat($"pointLights[{i}].linear", Linear);
      shader.SetFloat($"pointLights[{i}].quadratic", Quadratic);
    }
  }

}