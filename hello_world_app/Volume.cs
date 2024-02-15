using OpenTK.Mathematics;

namespace app {
  internal abstract class Volume {
    public float[]? Vertices { get; init; }
    public float[]? TexCoords {  get; init; }
    public uint[]? Indices { get; init; }
    public int Texture { get; set; }
    public float[]? Colors { get; init; }
    public OpenTK.Mathematics.Vector3 ScaleVr {  get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; }

    internal  Matrix4 ComputeModelMatrix() {
      return Matrix4.Identity *
        Matrix4.CreateScale(ScaleVr) * 
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(RotationVr.X)) * 
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(RotationVr.Y)) * 
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(RotationVr.Z)) * 
        Matrix4.CreateTranslation(PosVr);
    }
    
    public Volume() {
      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }

    public Volume(string path, OpenTK.Mathematics.Vector3 color) : this() {

      if (path != "") {
        FileData data = app.OBJParser.ParseFromFile(path);

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
        (Vertices, Indices, Colors) = app.Generator.GenerateCube(10, color);
      }
    }

  }
   
  internal class Cube : Volume {

    public Cube(int verticesInLine, OpenTK.Mathematics.Vector3 color) {
      (Vertices, Indices, Colors) = app.Generator.GenerateCube(verticesInLine, color);

      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }
  }
}


