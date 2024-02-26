using SimpleDrawing.Entities;
using System.Numerics;

public class FlashLight : Light {

  public Vector3 Direction;
  public Vector3 Color;
  public Vector3 Diffuse;
  public Vector3 Specular;

  public float CutOff;
  public float OuterCutOff;

  public float Constant;
  public float Linear;
  public float Quadratic;

  public FlashLight() {
    Form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
    Form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

    Direction = new Vector3(0.0f, 0.0f, 1.0f);

    Color = new Vector3(0.0f, 0.0f, 0.0f);
    Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
    Specular = new Vector3(1.0f, 1.0f, 1.0f);

    CutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(12.5f));
    OuterCutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(20.5f));

    Constant = 1.0f;
    Linear = 0.09f;
    Quadratic = 0.032f;
  }
  public override void AdjustShader(ref Shader shader, int i) {
    base.AdjustShader(ref shader, i);

    shader.SetUniform3($"flashLights[{i}].position", Form.PosVr);
    shader.SetUniform3($"flashLights[{i}].direction",
        new OpenTK.Mathematics.Vector3(Direction.X, Direction.Y, Direction.Z));
    shader.SetFloat($"flashLights[{i}].cutOff", CutOff);
    shader.SetFloat($"flashLights[{i}].outerCutOff", OuterCutOff);
    shader.SetUniform3($"flashLights[{i}].color",
        new OpenTK.Mathematics.Vector3(Color.X, Color.Y, Color.Z));
    shader.SetUniform3($"flashLights[{i}].diffuse",
        new OpenTK.Mathematics.Vector3(Diffuse.X, Diffuse.Y, Diffuse.Z));
    shader.SetUniform3($"flashLights[{i}].specular",
        new OpenTK.Mathematics.Vector3(Specular.X, Specular.Y, Specular.Z));
    shader.SetFloat($"flashLights[{i}].constant", Constant);
    shader.SetFloat($"flashLights[{i}].linear", Linear);
    shader.SetFloat($"flashLights[{i}].quadratic", Quadratic);
  }
}