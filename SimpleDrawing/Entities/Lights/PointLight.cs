using SimpleDrawing.Entities;
using System.Numerics;


public class PointLight : Light {

  public Vector3 _color;
  public Vector3 _diffuse;
  public Vector3 _specular;
  public float _constant;
  public float _linear;
  public float _quadratic;

  public PointLight() {
    _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
    _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

    _color = new Vector3(0.05f, 0.05f, 0.05f);
    _diffuse = new Vector3(0.8f, 0.8f, 0.8f);
    _specular = new Vector3(1.0f, 1.0f, 1.0f);

    _constant = 1.0f;
    _linear = 0.09f;
    _quadratic = 0.032f;
  }

  public override void AdjustShader(ref Shader shader, int i) {
    base.AdjustShader(ref shader, i);

    shader.SetUniform3($"pointLights[{i}].position", _form.PosVr);
    shader.SetUniform3($"pointLights[{i}].color",
      new OpenTK.Mathematics.Vector3(_color.X, _color.Y, _color.Z));
    shader.SetUniform3($"pointLights[{i}].diffuse",
      new OpenTK.Mathematics.Vector3(_diffuse.X, _diffuse.Y, _diffuse.Z));
    shader.SetUniform3($"pointLights[{i}].specular",
      new OpenTK.Mathematics.Vector3(_specular.X, _specular.Y, _specular.Z));
    shader.SetFloat($"pointLights[{i}].constant", _constant);
    shader.SetFloat($"pointLights[{i}].linear", _linear);
    shader.SetFloat($"pointLights[{i}].quadratic", _quadratic);
  }

}