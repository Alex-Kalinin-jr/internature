namespace SimpleDrawing.Model {

  public abstract class Light {
    public Cube ItsVolume;
    public virtual void AdjustShader(ref Shader shader, int i) { }
  }

}


