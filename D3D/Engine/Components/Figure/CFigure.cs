﻿
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CFigure : Component {
    public CMesh IamMesh;
    public CTransform IamTransform;
    public CPrimitiveTopology IamTopology;

    public CFigure(string path, 
                   VsMvpConstantBuffer matrix,
                   FigureType type = FigureType.General, 
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      IamMesh = new CMesh(path);
      IamTransform = new CTransform(matrix);
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   CTransform iamTransform, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      IamMesh = iamMesh;
      IamTransform = iamTransform;
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public CFigure(CMesh iamMesh, 
                   VsMvpConstantBuffer matrix, 
                   FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList) {
      IamMesh = iamMesh;
      IamTransform = new CTransform(matrix);
      IamTopology = new CPrimitiveTopology(topology);
      DrawSystem.Register(this, type);
    }

    public override void UpdateLinks() {
      IamMesh.IamEntity = IamEntity;
      IamTransform.IamEntity = IamEntity;
      IamTopology.IamEntity = IamEntity;
    }
  }
}
