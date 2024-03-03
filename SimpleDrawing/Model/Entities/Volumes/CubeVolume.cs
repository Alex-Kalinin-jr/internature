using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class Cube : Volume {
    public Cube(int verticesInLine) {
      Vao = -1;
      Texture = -1;
      ItsPosition = new Position();
      ItsMaterial = new MaterialColor();
      ItsForm = Generator.GenerateCubeForm(10);
    }
    public override void AdjustShader(ref Shader shader) {
      ColorAdjuster.AdjustShader(ref ItsMaterial, ref shader, 0);

      var modelMatrix = ComputeModelMatrix();
      shader.SetMatrix4("model", modelMatrix);
      modelMatrix.Invert();
      shader.SetMatrix4("invertedModel", modelMatrix);
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

