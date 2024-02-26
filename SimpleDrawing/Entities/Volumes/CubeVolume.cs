using SimpleDrawing.Entities;
using OpenTK.Mathematics;


public class Cube : Volume {

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