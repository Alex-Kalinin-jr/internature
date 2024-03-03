using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {

  public abstract class Volume : IBindable {

    public MaterialColor ItsMaterial { get; set; }
    public Position ItsPosition { get; set; }
    public Form ItsForm {  get; set; }
    public int Vao { get; set; }
    public int Texture { get; set; }
    public abstract void AdjustShader(ref Shader shader);
    public abstract OpenTK.Mathematics.Matrix4 ComputeModelMatrix();

    public void Bind(ref Shader shader) {

      Vao = GL.GenVertexArray();
      GL.BindVertexArray(Vao);

      if (ItsForm.Vertices != null) {
        GlBinder.BindPosBuffer(ItsForm.Vertices, ref shader, 3);
      }

      if (ItsForm.Normals != null) {
        GlBinder.BindNormalBuffer(ItsForm.Normals, ref shader);
      }
    }

  }
}


