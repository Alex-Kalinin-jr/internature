
namespace SimpleDrawing.Model {
  public class Cube : Volume {

    public Cube(int verticesInLine, OpenTK.Mathematics.Vector3 color) {
      (Vertices, MaterialTraits, Normals)
          = Generator.GenerateCube(verticesInLine, color);

      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }

    public Cube(string a) {
      (Vertices, MaterialTraits, Normals) = Generator.GenerateTestingCube();
      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }
  }

}

