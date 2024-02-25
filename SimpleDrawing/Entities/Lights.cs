using System.Numerics;




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
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

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

  internal class PointLight : Light {

    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set;}
    public float Constant {  get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }

    public PointLight() {
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

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
      _form = new Cube(4, new OpenTK.Mathematics.Vector3(1.0f, 1.0f, 1.0f));
      _form.ScaleVr = new OpenTK.Mathematics.Vector3(0.2f, 0.2f, 0.2f);

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

      shader.SetUniform3($"flashLights[{i}].position", _form.PosVr);
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
}


