using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {

  public abstract class Light : IBindable {

    public Cube ItsVolume;
    public virtual void AdjustShader(ref Shader shader, int i) { }

    public void Bind(ref Shader shader) {
      ItsVolume.Vao = GL.GenVertexArray();
      GL.BindVertexArray(ItsVolume.Vao);

      if (ItsVolume.ItsForm.Vertices != null) {
        GlBinder.BindPosBuffer(ItsVolume.ItsForm.Vertices, ref shader);
      }
    }


  }

}


