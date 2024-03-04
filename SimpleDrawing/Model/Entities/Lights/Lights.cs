using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {

  public class Light : IBindable {

    public Cube ItsVolume;
    public Color ItsColor;

    public Light() {
      ItsVolume = new Cube(4);
      ItsColor = new Color();
      ItsVolume.ItsPosition.ScaleVr = new OpenTK.Mathematics.Vector3(0.1f, 0.1f, 0.1f);
    }

    public void Bind(ref Shader shader) {
      ItsVolume.Vao = GL.GenVertexArray();
      GL.BindVertexArray(ItsVolume.Vao);

      if (ItsVolume.ItsForm.Vertices != null) {
        ShaderAdjuster.BindBuffer(ItsVolume.ItsForm.Vertices, ref shader, 3, "aPos");
      }
    }


  }

}


