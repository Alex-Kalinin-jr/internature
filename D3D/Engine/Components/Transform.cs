using SharpDX;

namespace D3D {
  public class CTransform : Component {
    public VsMvpConstantBuffer IamTransform;

    public CTransform(VsMvpConstantBuffer matr) {
      IamTransform = matr;
      TransformSystem.Register(this);
    }

  }

}
