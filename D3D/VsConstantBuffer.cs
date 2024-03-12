using SharpDX;

namespace D3D {
  public struct VS_CONSTANT_BUFFER {
    public Matrix world;
    public Matrix view;
    public Matrix projection;
    public Matrix modelMatrix;
  }
}
