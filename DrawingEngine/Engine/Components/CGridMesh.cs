using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CGridMesh : CMesh {

    public List<short> LineIndices;
    public Dictionary<string, float[]> Properties;

    public CGridMesh() : base() {
      LineIndices = new List<short>();
      Properties = new Dictionary<string, float[]>();
    }

    public CGridMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.Grid)
        : base(vertices, indices, figureType) {
      LineIndices = new List<short>();
      Properties = new Dictionary<string, float[]>();
    }

    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }

    public void AddProperty(string type, float[] values) {
      int vertexCountInOneGrid = 8;
      if (!Properties.ContainsKey(type) && Vertices.Count == values.Length * vertexCountInOneGrid) {
        Properties.Add(type, values);
      }
    }

    public List<string> GetProperties() {
      List<string> result = new List<string>();
      foreach (var prop in Properties.Keys) {
        result.Add(prop);
      }
      return result;
    }
  }
}
