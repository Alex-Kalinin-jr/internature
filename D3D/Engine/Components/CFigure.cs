
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CFigure : Component {
    public CMesh MeshObj;
    public CTransform TransformObj;
    public CPrimitiveTopology TopologyObj;

    public CFigure(string path, 
                   VsMvpConstantBuffer matrix,
                   FigureType type = FigureType.General, 
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = new CMesh(path);
      TransformObj = new CTransform(matrix);
      TopologyObj = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   CTransform iamTransform, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = iamMesh;
      TransformObj = iamTransform;
      TopologyObj = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   VsMvpConstantBuffer matrix, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      MeshObj = iamMesh;
      TransformObj = new CTransform(matrix);
      TopologyObj = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public override void UpdateLinks() {
      MeshObj.EntityObj = EntityObj;
      TransformObj.EntityObj = EntityObj;
      TopologyObj.EntityObj = EntityObj;
    }
  }
}
