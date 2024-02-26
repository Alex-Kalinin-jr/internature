namespace SimpleDrawing.Entities {

  public abstract class Light {
    public Volume _form;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

}


