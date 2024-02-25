using OpenTK.Mathematics;
using System.Reflection;

namespace SimpleDrawing.Entities {
  public struct Material {
    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }

    public float Shiness { get; set; }
    public Material() {
      Ambient = new Vector3(1.0f, 0.5f, 0.0f);
      Diffuse = new Vector3(1.0f, 0.5f, 0.31f);
      Specular = new Vector3(0.5f, 0.5f, 0.5f);
      Shiness = 0.32f;
    }
  }

  internal abstract class Volume {

    public Material MaterialTraits { get; set; }

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

  internal class Cube : Volume {

    public Cube(int verticesInLine, Vector3 color) {
      (Vertices, MaterialTraits, Normals)
          = Generator.GenerateCube(verticesInLine, color);

      Texture = -1;
      ScaleVr = Vector3.One;
      PosVr = Vector3.Zero;
      RotationVr = Vector3.Zero;
    }

    public Cube(string a) {
      (Vertices, MaterialTraits, Normals) = Generator.GenerateTestingCube();
      Texture = -1;
      ScaleVr = Vector3.One;
      PosVr = Vector3.Zero;
      RotationVr = Vector3.Zero;
    }
  }
}


