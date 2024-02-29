namespace SimpleDrawing.Model {

  public abstract class Light {
    public Volume Form;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

}


