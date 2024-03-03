
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
      if (shader.UniformLocations.ContainsKey("material.ambient")) {
        shader.SetUniform3("material.ambient", ItsMaterial.Ambient);
      }
      if (shader.UniformLocations.ContainsKey("material.diffuse")) {
        shader.SetUniform3("material.diffuse", ItsMaterial.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey("material.specular")) {
        shader.SetUniform3("material.specular", ItsMaterial.Specular);
      }
      if (shader.UniformLocations.ContainsKey("material.shiness")) {
        shader.SetFloat("material.shiness", ItsMaterial.Shiness);
      }

      var modelMatrix = ComputeModelMatrix();
      shader.SetMatrix4("model", modelMatrix);
      modelMatrix.Invert();
      shader.SetMatrix4("invertedModel", modelMatrix);
    }
    public override OpenTK.Mathematics.Matrix4 ComputeModelMatrix() {
      return OpenTK.Mathematics.Matrix4.Identity *
        OpenTK.Mathematics.Matrix4.CreateScale(ItsPosition.ScaleVr) *
        OpenTK.Mathematics.Matrix4.CreateRotationX(OpenTK.Mathematics.MathHelper.DegreesToRadians(ItsPosition.RotationVr.X)) *
        OpenTK.Mathematics.Matrix4.CreateRotationY(OpenTK.Mathematics.MathHelper.DegreesToRadians(ItsPosition.RotationVr.Y)) *
        OpenTK.Mathematics.Matrix4.CreateRotationZ(OpenTK.Mathematics.MathHelper.DegreesToRadians(ItsPosition.RotationVr.Z)) *
        OpenTK.Mathematics.Matrix4.CreateTranslation(ItsPosition.PosVr);
    }
  }

}

