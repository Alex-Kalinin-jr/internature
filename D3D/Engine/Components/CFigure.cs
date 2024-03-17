
using SharpDX;

namespace D3D {
  public class CFigure : Component {
    public CMesh IamMesh;
    public CTransform IamTransform;

    public CFigure(string path, VsMvpConstantBuffer matr) {
      IamMesh = new CMesh(path);
      IamTransform = new CTransform(matr);
      DrawSystem.Register(this);
    }
  }
}
