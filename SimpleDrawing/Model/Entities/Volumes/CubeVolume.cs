using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class Cube : Volume {
    public Cube(int verticesInLine) {
      Vao = -1; // index -1 means that VAO is not assigned
      Texture = -1; // index -1 means that Texture is not assigned
      ItsPosition = new Position();
      ItsMaterial = new MaterialColor();
      ItsForm = Generator.GenerateCubeForm(10); // generates cube with faces of 10x10
    }

    public override Matrix4 ComputeModelMatrix() {
      return Matrix4.Identity * Matrix4.CreateScale(ItsPosition.ScaleVr) *
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(ItsPosition.RotationVr.X)) *
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ItsPosition.RotationVr.Y)) *
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(ItsPosition.RotationVr.Z)) *
        Matrix4.CreateTranslation(ItsPosition.PosVr);
    }
  }

}

