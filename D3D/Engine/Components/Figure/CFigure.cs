
using SharpDX;

namespace D3D {
  public class CFigure : Component {
    public CMesh IamMesh;
    public CTransform IamTransform;

    public CFigure(string path, VsMvpConstantBuffer matrix, FigureType type = FigureType.General) {
      IamMesh = new CMesh(path);
      IamTransform = new CTransform(matrix);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, CTransform iamTransform, FigureType type = FigureType.General) {
      IamMesh = iamMesh;
      IamTransform = iamTransform;
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, VsMvpConstantBuffer matrix, FigureType type = FigureType.General) {
      IamMesh = iamMesh;
      IamTransform = new CTransform(matrix);
      DrawSystem.Register(this, type);
    }

    public override void UpdateLinks() {
      IamMesh.IamEntity = IamEntity;
      IamTransform.IamEntity = IamEntity;
    }
  }
}
