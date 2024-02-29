
namespace SimpleDrawing.Model {
  public interface IMover {
    void Move(ref IPositioned source, IPositioned target);
  }
}
