using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {
  public class ShaderAdjuster {

    public enum mode {
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

    public static void AdjustShader(ref Shader shader, float val, mode flag) {
      if (flag == mode.morphingFactor && shader.UniformLocations.ContainsKey("morphingFactor")) {
        shader.SetFloat("morphingFactor", val);
      }
    }

    public static void AdjustShader(ref Shader shader, Vector3 val, int pos, mode flag) {
      if (flag == mode.position && shader.UniformLocations.ContainsKey($"flashLights[{pos}].position")) {
        shader.SetUniform3($"flashLights[{pos}].position", val);
      }
    }

    public static void AdjustShader(ref Shader shader, Vector3 val, mode flag) {
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

  }
}