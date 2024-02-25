using OpenTK.Mathematics;

namespace SimpleDrawing.Entities {
  internal struct DirectionalLight {
    public Cube _form;
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
  }

  internal struct PointLight {
    public Cube _form;

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
  }

  internal struct FlashLight {
    public Cube _form;

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

  }
}