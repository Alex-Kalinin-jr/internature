using SharpDX.Direct3D;
using System.Collections.Generic;

namespace D3D {
  public class CGridFigure : CFigure {
    public int XCount = -1;
    public int YCount = -1;
    public int ZCount = -1;

    public int CurrentXCount = -1;
    public int CurrentYCount = -1;
    public int CurrentZCount = -1;

    public List<short> FullIndices;

    public CGridFigure(string path, VsMvpConstantBuffer matrix, FigureType type = FigureType.General,
                       PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(path, matrix, type, topology) {
      FullIndices = base.IamMesh.Indices;
    }

    public CGridFigure(CMesh iamMesh, CTransform iamTransform, FigureType type = FigureType.General,
                       PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(iamMesh, iamTransform, type, topology) {
      FullIndices = base.IamMesh.Indices;
    }

    public CGridFigure(CMesh iamMesh, VsMvpConstantBuffer matrix, FigureType type = FigureType.General,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(iamMesh, matrix, type, topology) {
      FullIndices = base.IamMesh.Indices;
    }
  }
}
