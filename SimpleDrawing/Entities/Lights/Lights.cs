namespace SimpleDrawing.Entities {

  public abstract class Light {
    public Volume Form;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

}


