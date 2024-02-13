using OpenTK;
using OpenTK.Graphics;


namespace Ex {
  public struct Vertex {
    public const int Size = (4 + 4) * 4;

    private readonly OpenTK.Mathematics.Vector4 _position;
    private readonly OpenTK.Mathematics.Color4 _color;

    public Vertex(OpenTK.Mathematics.Vector4 position, OpenTK.Mathematics.Color4 color) {
      _position = position;
      _color = color;
    }
  }
}
