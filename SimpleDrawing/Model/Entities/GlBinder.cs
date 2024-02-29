using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {
  public class GlBinder {
    public static void BindPosBuffer(float[] vertices, ref Shader shader) {
      int vertexLocation = GL.GetAttribLocation(shader.Handle, "aPos");
      GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
          vertices, BufferUsageHint.DynamicDraw);
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
          false, 3 * sizeof(float), 0);
    }

    public static void BindNormalBuffer(float[] normals, ref Shader shader) {
      int normalLocation = GL.GetAttribLocation(shader.Handle, "aNormal");
      GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
      GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float),
          normals, BufferUsageHint.StaticDraw);
      GL.EnableVertexAttribArray(normalLocation);
      GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float,
          false, 3 * sizeof(float), 0);
    }

  }
}
