namespace SimpleDrawing.Entities {

  public abstract class Volume {

    public Material MaterialTraits { get; set; }

    public int Vao { get; set; }

    public float[]? Vertices { get; init; }
    public float[]? TexCoords { get; init; }
    public uint[]? Indices { get; init; }
    public int Texture { get; set; }

    public float[]? Normals { get; init; }
    public OpenTK.Mathematics.Vector3 ScaleVr { get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; }


    public Volume() {
      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
      MaterialTraits = new Material();
    }


    public Volume(string path, OpenTK.Mathematics.Vector3 color) : this() {

      (Vertices, MaterialTraits, Normals) = Generator.GenerateCube(10, color);

    }


    public void AdjustShader(ref Shader shader) {
      shader.SetUniform3("material.ambient", MaterialTraits.Ambient);
      shader.SetUniform3("material.diffuse", MaterialTraits.Diffuse);
      shader.SetUniform3("material.specular", MaterialTraits.Specular);
      shader.SetFloat("material.shiness", MaterialTraits.Shiness);

      var modelMatrix = ComputeModelMatrix();
      shader.SetMatrix4("model", modelMatrix);
      modelMatrix.Invert();
      shader.SetMatrix4("invertedModel", modelMatrix);
    }


    public OpenTK.Mathematics.Matrix4 ComputeModelMatrix() {
      return OpenTK.Mathematics.Matrix4.Identity *
        OpenTK.Mathematics.Matrix4.CreateScale(ScaleVr) *
        OpenTK.Mathematics.Matrix4.CreateRotationX(OpenTK.Mathematics.MathHelper.DegreesToRadians(RotationVr.X)) *
        OpenTK.Mathematics.Matrix4.CreateRotationY(OpenTK.Mathematics.MathHelper.DegreesToRadians(RotationVr.Y)) *
        OpenTK.Mathematics.Matrix4.CreateRotationZ(OpenTK.Mathematics.MathHelper.DegreesToRadians(RotationVr.Z)) *
        OpenTK.Mathematics.Matrix4.CreateTranslation(PosVr);
    }

  }

}


