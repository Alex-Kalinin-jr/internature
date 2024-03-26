using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CGridMesh : CMesh {
    public List<short> LineIndices;

    public CGridMesh(string path) : base(path) {
      LineIndices = new List<short>();
    }

    public CGridMesh() : base() {
      LineIndices = new List<short>();
    }

    public CGridMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.Grid) 
        : base (vertices, indices, figureType) {

    }

    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }

  }
}
