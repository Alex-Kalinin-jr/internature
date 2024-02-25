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

    public Vector3 _color { get; set; }
    public Vector3 _diffuse { get; set; }
    public Vector3 _specular { get; set;}
    public float Constant {  get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }

    public PointLight() {
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

      _color = new Vector3(0.05f, 0.05f, 0.05f);
      _diffuse = new Vector3(0.8f, 0.8f, 0.8f);
      _specular = new Vector3(1.0f, 1.0f, 1.0f);

      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
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
      shader.SetFloat($"pointLights[{i}].constant", Constant);
      shader.SetFloat($"pointLights[{i}].linear", Linear);
      shader.SetFloat($"pointLights[{i}].quadratic", Quadratic);
    }

  }

  internal class FlashLight : Light {

    public Vector3 _direction;
    public Vector3 _color;
    public Vector3 _diffuse;
    public Vector3 _specular;

    public float CutOff;
    public float OuterCutOff;

    public float Constant;
    public float Linear;
    public float Quadratic;

    public FlashLight() {
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

      _direction = new Vector3(0.0f, 0.0f, 1.0f);

      _color = new Vector3(0.0f, 0.0f, 0.0f);
      _diffuse = new Vector3(1.0f, 1.0f, 1.0f);
      _specular = new Vector3(1.0f, 1.0f, 1.0f);

      CutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(12.5f));
      OuterCutOff = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(20.5f));

      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
    }

    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"flashLights[{i}].position", _form.PosVr);
      shader.SetUniform3($"flashLights[{i}].direction", 
          new OpenTK.Mathematics.Vector3(_direction.X, _direction.Y, _direction.Z));
      shader.SetFloat($"flashLights[{i}].cutOff", CutOff);
      shader.SetFloat($"flashLights[{i}].outerCutOff", OuterCutOff);
      shader.SetUniform3($"flashLights[{i}].color", 
          new OpenTK.Mathematics.Vector3(_color.X, _color.Y, _color.Z));
      shader.SetUniform3($"flashLights[{i}].diffuse", 
          new OpenTK.Mathematics.Vector3(_diffuse.X, _diffuse.Y, _diffuse.Z));
      shader.SetUniform3($"flashLights[{i}].specular", 
          new OpenTK.Mathematics.Vector3(_specular.X, _specular.Y, _specular.Z));
      shader.SetFloat($"flashLights[{i}].constant", Constant);
      shader.SetFloat($"flashLights[{i}].linear", Linear);
      shader.SetFloat($"flashLights[{i}].quadratic", Quadratic);
    }
  }
}


