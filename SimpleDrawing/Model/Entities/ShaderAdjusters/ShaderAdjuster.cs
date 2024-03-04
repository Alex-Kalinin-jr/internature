using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class ShaderAdjuster {
    public static void AdjustShader(ref Camera camera, ref Shader shader) {
      if (shader.UniformLocations.ContainsKey("viewPos")) {
        shader.SetUniform3("viewPos", camera.Position);

      }
      if (shader.UniformLocations.ContainsKey("view")) {
        shader.SetMatrix4("view", camera.GetViewMatrix());

      }
      if (shader.UniformLocations.ContainsKey("projection")) {
        shader.SetMatrix4("projection", camera.GetProjectionMatrix());
      }
    }

    public static void AdjustShader(ref Shader shader, float val, int flag) {
      if (flag == 0 && shader.UniformLocations.ContainsKey("morphingFactor")) {
        shader.SetFloat("morphingFactor", val);
      }
    }

    public static void AdjustShader(ref Shader shader, Vector3 val, int pos, int flag) {
        if (flag == 0 && shader.UniformLocations.ContainsKey($"flashLights[{pos}].position")) {
        shader.SetUniform3($"flashLights[{pos}].position", val);
      }
    }

    public static void AdjustShader(ref Matrix4 modelMatrix, ref Shader shader, int flag) {
      if (flag == 0 && shader.UniformLocations.ContainsKey("model")) {
        shader.SetMatrix4("model", modelMatrix);
      }
      if (shader.UniformLocations.ContainsKey("invertedModel")) {
        modelMatrix.Invert();
        shader.SetMatrix4("invertedModel", modelMatrix);
      }
    }

  }
}