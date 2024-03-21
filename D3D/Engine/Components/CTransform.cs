namespace D3D {
  public class CTransform : Component {
    public VsMvpConstantBuffer TransformObj;

    public CTransform(VsMvpConstantBuffer matrix) {
      TransformObj = matrix;
      TransformSystem.Register(this);
    }

  }

}
