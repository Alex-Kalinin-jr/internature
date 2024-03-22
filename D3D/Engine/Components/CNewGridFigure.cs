using SharpDX.Direct3D;

namespace D3D {
  public class CNewGridFigure : CFigure {
    public int XCoord;
    public int YCoord;
    public int ZCoord;

    public CNewGridFigure(string path, VsMvpConstantBuffer matrix, FigureType type = FigureType.Grid,
                       PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(path, matrix, type, topology) {
    }

    public CNewGridFigure(CMesh iamMesh, CTransform iamTransform, FigureType type = FigureType.Grid,
                       PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(iamMesh, iamTransform, type, topology) {
    }

    public CNewGridFigure(CMesh iamMesh, VsMvpConstantBuffer matrix, FigureType type = FigureType.Grid,
                   PrimitiveTopology topology = PrimitiveTopology.TriangleList)
        : base(iamMesh, matrix, type, topology) {
    }
  }
}
