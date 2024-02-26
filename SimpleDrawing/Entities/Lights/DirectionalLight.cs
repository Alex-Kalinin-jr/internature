using SimpleDrawing.Entities;
using System.Numerics;

public class DirectionalLight : Light {
  public Vector3 Direction;
  public Vector3 Color;
  public Vector3 Diffuse;
  public Vector3 Specular;

  public DirectionalLight() {
    Form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
    Form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

    Direction = new Vector3(-0.2f, -1.0f, -0.3f);

    Color = new Vector3(0.05f, 0.05f, 0.05f);
    Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
    Specular = new Vector3(0.5f, 0.5f, 0.5f);
  }

  public override void AdjustShader(ref Shader shader, int i) {
    base.AdjustShader(ref shader, i);
    shader.SetUniform3($"dirlights[{i}].direction",
        new OpenTK.Mathematics.Vector3(Direction.X, Direction.Y, Direction.Z));
    shader.SetUniform3($"dirlights[{i}].color",
        new OpenTK.Mathematics.Vector3(Color.X, Color.Y, Color.Z));
    shader.SetUniform3($"dirlights[{i}].diffuse",
        new OpenTK.Mathematics.Vector3(Diffuse.X, Diffuse.Y, Diffuse.Z));
    shader.SetUniform3($"dirlights[{i}].specular",
        new OpenTK.Mathematics.Vector3(Specular.X, Specular.Y, Specular.Z));
  }

}