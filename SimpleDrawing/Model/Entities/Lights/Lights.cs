using OpenTK.Graphics.OpenGL4;
using SimpleDrawing.Model.Entities.ShaderAdjusters;

namespace SimpleDrawing.Model {

  public class Light : IBindable {

    public Cube ItsVolume;
    public Color ItsColor;

    public Light() {
      ItsVolume = new Cube(4);
      ItsColor = new Color();
      ItsVolume.ItsPosition.ScaleVr = new OpenTK.Mathematics.Vector3(0.1f, 0.1f, 0.1f);
    }

    public void AdjustShader(ref Shader shader, int i) {
        ColorAdjuster.AdjustShader(ref ItsColor, ref shader, i);
    }

    public void Bind(ref Shader shader) {
      ItsVolume.Vao = GL.GenVertexArray();
      GL.BindVertexArray(ItsVolume.Vao);

      if (ItsVolume.ItsForm.Vertices != null) {
        GlBinder.BindPosBuffer(ItsVolume.ItsForm.Vertices, ref shader, 3);
      }
    }


  }

}


