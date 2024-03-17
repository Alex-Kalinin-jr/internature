using SharpDX;

namespace D3D {
  public class CTransform : Component {
    public VsMvpConstantBuffer IamTransform;

    public CTransform() {
      TransformSystem.Register(this);
    } 
  }

}
