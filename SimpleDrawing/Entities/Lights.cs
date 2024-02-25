using OpenTK.Mathematics;




namespace SimpleDrawing.Entities {

  internal abstract class Light {
    public Volume _form;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

  internal class DirectionalLight : Light {
    public Vector3 Direction { get; set; }
    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }

    public DirectionalLight() {
      _form = new Cube(4, new Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new Vector3(0.2f, 0.2f, 0.2f);

      Direction = new Vector3(-0.2f, -1.0f, -0.3f);

      Color = new Vector3(0.05f, 0.05f, 0.05f);
      Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
      Specular = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"dirlights[{i}].direction", Direction);
      shader.SetUniform3($"dirlights[{i}].color", Color);
      shader.SetUniform3($"dirlights[{i}].diffuse", Diffuse);
      shader.SetUniform3($"dirlights[{i}].specular", Specular);
    }

  }

  internal class PointLight : Light {

    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set;}
    public float Constant {  get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }

    public PointLight() {
      _form = new Cube(4, new Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new Vector3(0.2f, 0.2f, 0.2f);

      Color = new Vector3(0.05f, 0.05f, 0.05f);
      Diffuse = new Vector3(0.8f, 0.8f, 0.8f);
      Specular = new Vector3(1.0f, 1.0f, 1.0f);

      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
    }

    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"pointLights[{i}].position", _form.PosVr);
      shader.SetUniform3($"pointLights[{i}].color", Color);
      shader.SetUniform3($"pointLights[{i}].diffuse", Diffuse);
      shader.SetUniform3($"pointLights[{i}].specular", Specular);
      shader.SetFloat($"pointLights[{i}].constant", Constant);
      shader.SetFloat($"pointLights[{i}].linear", Linear);
      shader.SetFloat($"pointLights[{i}].quadratic", Quadratic);
    }

  }

  internal class FlashLight : Light {

    public Vector3 Direction { get; set; }
    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set;}

    public float CutOff {  get; set; }
    public float OuterCutOff { get; set; }

    public float Constant { get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }

    public FlashLight() {
      _form = new Cube(4, new Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new Vector3(0.2f, 0.2f, 0.2f);

      Direction = new Vector3(0.0f, 0.0f, 1.0f);

      Color = new Vector3(0.0f, 0.0f, 0.0f);
      Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
      Specular = new Vector3(1.0f, 1.0f, 1.0f);

      CutOff = MathF.Cos(MathHelper.DegreesToRadians(12.5f));
      OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(20.5f));

      Constant = 1.0f;
      Linear = 0.09f;
      Quadratic = 0.032f;
    }

    public override void AdjustShader(ref Shader shader, int i) {
      base.AdjustShader(ref shader, i);

      shader.SetUniform3($"flashLights[{i}].position", _form.PosVr);
      shader.SetUniform3($"flashLights[{i}].direction", Direction);
      shader.SetFloat($"flashLights[{i}].cutOff", CutOff);
      shader.SetFloat($"flashLights[{i}].outerCutOff", OuterCutOff);
      shader.SetUniform3($"flashLights[{i}].color", Color);
      shader.SetUniform3($"flashLights[{i}].diffuse", Diffuse);
      shader.SetUniform3($"flashLights[{i}].specular", Specular);
      shader.SetFloat($"flashLights[{i}].constant", Constant);
      shader.SetFloat($"flashLights[{i}].linear", Linear);
      shader.SetFloat($"flashLights[{i}].quadratic", Quadratic);
    }
  }
}