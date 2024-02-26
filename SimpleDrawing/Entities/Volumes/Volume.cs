using OpenTK.Mathematics;

namespace SimpleDrawing.Entities {

  public abstract class Volume {

    public Material MaterialTraits { get; set; }

    public int VAO { get; set; }

    public float[]? Vertices { get; init; }
    public float[]? TexCoords { get; init; }
    public uint[]? Indices { get; init; }
    public int Texture { get; set; }

    public float[]? Normals { get; init; }
    public Vector3 ScaleVr { get; set; }
    public Vector3 PosVr { get; set; }
    public Vector3 RotationVr { get; set; }

    internal Matrix4 ComputeModelMatrix() {
      return Matrix4.Identity *
        Matrix4.CreateScale(ScaleVr) *
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(RotationVr.X)) *
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(RotationVr.Y)) *
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(RotationVr.Z)) *
        Matrix4.CreateTranslation(PosVr);
    }

    public Volume() {
      Texture = -1;
      ScaleVr = Vector3.One;
      PosVr = Vector3.Zero;
      RotationVr = Vector3.Zero;
      MaterialTraits = new Material();
    }

    public Volume(string path, Vector3 color) : this() {

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

  }

}


