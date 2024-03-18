using SharpDX;

namespace D3D {
  public class CTransform : Component {
    public VsMvpConstantBuffer IamTransform;

    public CTransform(VsMvpConstantBuffer matrix) {
      IamTransform = matrix;
      TransformSystem.Register(this);
    }

  }

}
