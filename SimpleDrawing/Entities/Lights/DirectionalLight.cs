using SimpleDrawing.Entities;
using System.Numerics;

public class DirectionalLight : Light {
  public Vector3 _direction;
  public Vector3 _color;
  public Vector3 _diffuse;
  public Vector3 _specular;

  public DirectionalLight() {
    _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
    _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

    _direction = new Vector3(-0.2f, -1.0f, -0.3f);

    _color = new Vector3(0.05f, 0.05f, 0.05f);
    _diffuse = new Vector3(0.4f, 0.4f, 0.4f);
    _specular = new Vector3(0.5f, 0.5f, 0.5f);
  }

  public override void AdjustShader(ref Shader shader, int i) {
    base.AdjustShader(ref shader, i);
    shader.SetUniform3($"dirlights[{i}].direction",
        new OpenTK.Mathematics.Vector3(_direction.X, _direction.Y, _direction.Z));
    shader.SetUniform3($"dirlights[{i}].color",
        new OpenTK.Mathematics.Vector3(_color.X, _color.Y, _color.Z));
    shader.SetUniform3($"dirlights[{i}].diffuse",
        new OpenTK.Mathematics.Vector3(_diffuse.X, _diffuse.Y, _diffuse.Z));
    shader.SetUniform3($"dirlights[{i}].specular",
        new OpenTK.Mathematics.Vector3(_specular.X, _specular.Y, _specular.Z));
  }

}