using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleDrawing.Model {

  public abstract class Volume : IBindable {

    public Color ItsMaterial;
    public Position ItsPosition { get; set; }
    public Form ItsForm {  get; set; }
    public int Vao { get; set; }
    public int Texture { get; set; }
    public abstract Matrix4 ComputeModelMatrix();

    public void Bind(ref Shader shader) {
      if (Vao == -1) {
        Vao = GL.GenVertexArray();
      }
      GL.BindVertexArray(Vao);

      if (ItsForm.Vertices != null) {
        ShaderAdjuster.BindBuffer(ItsForm.Vertices, ref shader, 3, "aPos");
      }

      if (ItsForm.Normals != null) {
        ShaderAdjuster.BindBuffer(ItsForm.Normals, ref shader, 3, "aNormal");
      }
    }

  }
}


