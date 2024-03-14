using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {
  public class ShaderAdjuster {

    public enum mode {
      notUsed,
      position,
      color,
      modelmatrix,
      morphingFactor
    }

    public static void BindBuffer(float[] vertices, ref Shader shader, int numOfComponents, string str) {
      int vertexLocation = GL.GetAttribLocation(shader.Handle, str);
      GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
          vertices, BufferUsageHint.StaticDraw);
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, numOfComponents, 
          VertexAttribPointerType.Float, false, numOfComponents * sizeof(float), 0);
    }

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

    public static void AdjustShader(float val, ref Shader shader, mode flag) {
      if (flag == mode.morphingFactor && shader.UniformLocations.ContainsKey("morphingFactor")) {
        shader.SetFloat("morphingFactor", val);
      }
    }

    public static void AdjustShader(Vector3 val, ref Shader shader, int pos, mode flag) {
      if (flag == mode.position && shader.UniformLocations.ContainsKey($"flashLights[{pos}].position")) {
        shader.SetUniform3($"flashLights[{pos}].position", val);
      }
      if (flag == mode.color && shader.UniformLocations.ContainsKey("Color")) {
        shader.SetUniform3("Color", val);
      }
    }

    public static void AdjustShader(ref Matrix4 modelMatrix, ref Shader shader, mode flag) {
      if (flag == mode.modelmatrix && shader.UniformLocations.ContainsKey("model")) {
        shader.SetMatrix4("model", modelMatrix);
        
        if (shader.UniformLocations.ContainsKey("invertedModel")) {
          modelMatrix.Invert();
          shader.SetMatrix4("invertedModel", modelMatrix);
        }
      }
    }

    public static void AdjustShader(Color color, ref Shader shader, int pos) {
      if (color is FlashLightColor fL) {
        AdjustFlashLight(ref fL, ref shader, pos);
      } else if (color is PointLightColor pL) {
        AdjustPointLight(ref pL, ref shader, pos);
      } else if (color is DirectionalLightColor dL) {
        AdjustDirLight(ref dL, ref shader, pos);
      } else if (color is MaterialColor mC) {
        AdjustMaterial(ref mC, ref shader);
      }
    }

    private static void AdjustDirLight(ref DirectionalLightColor dL, ref Shader shader, int i) {
      if (shader.UniformLocations.ContainsKey($"dirlights[{i}].direction")) {
        shader.SetUniform3($"dirlights[{i}].direction", dL.Direction);
      }
      if (shader.UniformLocations.ContainsKey($"dirlights[{i}].color")) {
        shader.SetUniform3($"dirlights[{i}].color", dL.Ambient);
      }
      if (shader.UniformLocations.ContainsKey($"dirlights[{i}].diffuse")) {
        shader.SetUniform3($"dirlights[{i}].diffuse", dL.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey($"dirlights[{i}].specular")) {
        shader.SetUniform3($"dirlights[{i}].specular", dL.Specular);
      }
    }

    private static void AdjustPointLight(ref PointLightColor color, ref Shader shader, int i) {
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].color")) {
        shader.SetUniform3($"pointLights[{i}].color", color.Ambient);
      }
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].diffuse")) {
        shader.SetUniform3($"pointLights[{i}].diffuse", color.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].specular")) {
        shader.SetUniform3($"pointLights[{i}].specular", color.Specular);
      }
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].constant")) {
        shader.SetFloat($"pointLights[{i}].constant", color.Constant);
      }
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].linear")) {
        shader.SetFloat($"pointLights[{i}].linear", color.Linear);
      }
      if (shader.UniformLocations.ContainsKey($"pointLights[{i}].quadratic")) {
        shader.SetFloat($"pointLights[{i}].quadratic", color.Quadratic);
      }
    }

    private static void AdjustFlashLight(ref FlashLightColor color, ref Shader shader, int i) {
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].direction")) {
        shader.SetUniform3($"flashLights[{i}].direction", color.Direction);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].color")) {
        shader.SetUniform3($"flashLights[{i}].color", color.Ambient);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].diffuse")) {
        shader.SetUniform3($"flashLights[{i}].diffuse", color.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].specular")) {
        shader.SetUniform3($"flashLights[{i}].specular", color.Specular);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].cutOff")) {
        shader.SetFloat($"flashLights[{i}].cutOff", color.CutOff);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].outerCutOff")) {
        shader.SetFloat($"flashLights[{i}].outerCutOff", color.OuterCutOff);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].constant")) {
        shader.SetFloat($"flashLights[{i}].constant", color.Constant);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].linear")) {
        shader.SetFloat($"flashLights[{i}].linear", color.Linear);
      }
      if (shader.UniformLocations.ContainsKey($"flashLights[{i}].quadratic")) {
        shader.SetFloat($"flashLights[{i}].quadratic", color.Quadratic);
      }
    }

    private static void AdjustMaterial(ref MaterialColor color, ref Shader shader) {
      if (shader.UniformLocations.ContainsKey("material.ambient")) {
        shader.SetUniform3("material.ambient", color.Ambient);
      }
      if (shader.UniformLocations.ContainsKey("material.diffuse")) {
        shader.SetUniform3("material.diffuse", color.Diffuse);
      }
      if (shader.UniformLocations.ContainsKey("material.specular")) {
        shader.SetUniform3("material.specular", color.Specular);
      }
      if (shader.UniformLocations.ContainsKey("material.shiness")) {
        shader.SetFloat("material.shiness", color.Shiness);
      }
    }
  }
}