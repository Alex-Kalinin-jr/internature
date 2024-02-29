namespace SimpleDrawing.Model {

  public abstract class Volume {

    public Material ItsMaterial { get; set; }
    public Position ItsPosition { get; set; }
    public Form ItsForm {  get; set; }
    public int Vao { get; set; }
    public int Texture { get; set; }
    public abstract void AdjustShader(ref Shader shader);
    public abstract OpenTK.Mathematics.Matrix4 ComputeModelMatrix();

  }

}


