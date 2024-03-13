using SharpDX;

namespace D3D {
  public struct VsMvpConstantBuffer {
    public Matrix world;
    public Matrix view;
    public Matrix projection;
    public Matrix modelMatrix;
  }
}
