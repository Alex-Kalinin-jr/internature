using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CGridMesh : CMesh {

    public List<short> LineIndices;
    public Dictionary<string, Vector3[]> Properties;

    public CGridMesh() : base() {
      LineIndices = new List<short>();
      Properties = new Dictionary<string, Vector3[]>();
    }

    public CGridMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.Grid) 
        : base (vertices, indices, figureType) {
      LineIndices = new List<short>();
      Properties = new Dictionary<string, Vector3[]>();
    }

    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }

    public void AddProperty(string type, Vector3[] values) {
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

    public void SetProperty(string type) {
      if (Properties.ContainsKey(type)) {
        int vertexCountInOneGrid = 8;
        var prop = Properties[type];
        int counter = 0;
        for (int i = 0; i < Vertices.Count; i += vertexCountInOneGrid) {
          for (int j = 0; j < vertexCountInOneGrid; ++j) {
            var v = Vertices[i + j];
            v.Color = prop[counter];
            Vertices[i + j] = v;
          }
          ++counter;
        }
      }
    }
  }
}
