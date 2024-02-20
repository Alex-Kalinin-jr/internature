using OpenGLInvestigation.Entities;
using OpenTK.Mathematics;

namespace OpenGLInvestigation.Figures {
  internal abstract class Volume {
    public float[]? Vertices { get; init; }
    public float[]? TexCoords { get; init; }
    public uint[]? Indices { get; init; }
    public int Texture { get; set; }
    public float[]? Colors { get; init; }
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
    }

    public Volume(string path, Vector3 color) : this() {

      if (path != "") {
        FileData data = OBJParser.ParseFromFile(path);

        if (data._vertices != null) {
          Vertices = data._vertices;
        }

        if (data._texCoords != null) {
          TexCoords = data._texCoords;
        }

        if (data._indices != null) {
          Indices = data._indices;
        }

      }

      if (path == null) {
        (Vertices, Indices, Colors, Normals) = Generator.GenerateCube(10, color);
      }
    }

  }

  internal class Cube : Volume {

    public Cube(int verticesInLine, Vector3 color) {
      (Vertices, Indices, Colors, Normals) = Generator.GenerateCube(verticesInLine, color);

      Texture = -1;
      ScaleVr = Vector3.One;
      PosVr = Vector3.Zero;
      RotationVr = Vector3.Zero;
    }
  }
}


