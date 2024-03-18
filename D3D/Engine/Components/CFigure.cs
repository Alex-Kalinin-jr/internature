
using SharpDX;

namespace D3D {
  public class CFigure : Component {
    public CMesh IamMesh;
    public CTransform IamTransform;

    public CFigure(string path, VsMvpConstantBuffer matrix) {
      IamMesh = new CMesh(path);
      IamTransform = new CTransform(matrix);
      DrawSystem.Register(this);
    }

    public CFigure(CMesh iamMesh, CTransform iamTransform) {
      IamMesh = iamMesh;
      IamTransform = iamTransform;
      DrawSystem.Register(this);
    }

    public CFigure(CMesh iamMesh, VsMvpConstantBuffer matrix) {
      IamMesh = iamMesh;
      IamTransform = new CTransform(matrix);
      DrawSystem.Register(this);
    }

    public override void UpdateLinks() {
      IamMesh.IamEntity = IamEntity;
      IamTransform.IamEntity = IamEntity;
    }
  }
}
