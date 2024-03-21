
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CFigure : Component {
    public CMesh MeshObj;
    public CTransform IamTransform;
    public CPrimitiveTopology IamTopology;

    public CFigure(string path, 
                   VsMvpConstantBuffer matrix,
                   FigureType type = FigureType.General, 
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = new CMesh(path);
      IamTransform = new CTransform(matrix);
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   CTransform iamTransform, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = iamMesh;
      IamTransform = iamTransform;
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   VsMvpConstantBuffer matrix, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = iamMesh;
      IamTransform = new CTransform(matrix);
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public override void UpdateLinks() {
      MeshObj.IamEntity = IamEntity;
      IamTransform.IamEntity = IamEntity;
      IamTopology.IamEntity = IamEntity;
    }
  }
}
