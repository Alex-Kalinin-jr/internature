using System.Numerics;




namespace SimpleDrawing.Entities {

  internal abstract class Light {
    public Volume _form;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

  internal class DirectionalLight : Light {
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

  internal class PointLight : Light {

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

  internal class FlashLight : Light {

    public Vector3 _direction;
    public Vector3 _color;
    public Vector3 _diffuse;
    public Vector3 _specular;

    public float _cutOff;
    public float _outerCutOff;

    public float _constant;
    public float _linear;
    public float _quadratic;

    public FlashLight() {
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

      _direction = new Vector3(0.0f, 0.0f, 1.0f);

      _color = new Vector3(0.0f, 0.0f, 0.0f);
      _diffuse = new Vector3(1.0f, 1.0f, 1.0f);
      _specular = new Vector3(1.0f, 1.0f, 1.0f);

      _cutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(12.5f));
      _outerCutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(20.5f));

      _constant = 1.0f;
      _linear = 0.09f;
      _quadratic = 0.032f;
    }
    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"flashLights[{i}].position", _form.PosVr);
      shader.SetUniform3($"flashLights[{i}].direction",
          new OpenTK.Mathematics.Vector3(_direction.X, _direction.Y, _direction.Z));
      shader.SetFloat($"flashLights[{i}].cutOff", _cutOff);
      shader.SetFloat($"flashLights[{i}].outerCutOff", _outerCutOff);
      shader.SetUniform3($"flashLights[{i}].color",
          new OpenTK.Mathematics.Vector3(_color.X, _color.Y, _color.Z));
      shader.SetUniform3($"flashLights[{i}].diffuse",
          new OpenTK.Mathematics.Vector3(_diffuse.X, _diffuse.Y, _diffuse.Z));
      shader.SetUniform3($"flashLights[{i}].specular",
          new OpenTK.Mathematics.Vector3(_specular.X, _specular.Y, _specular.Z));
      shader.SetFloat($"flashLights[{i}].constant", _constant);
      shader.SetFloat($"flashLights[{i}].linear", _linear);
      shader.SetFloat($"flashLights[{i}].quadratic", _quadratic);
    }
  }
}


