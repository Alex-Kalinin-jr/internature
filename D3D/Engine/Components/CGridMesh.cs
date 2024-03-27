using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CGridMesh : CMesh {
    public enum PropertyType {
      Color,
      Stability
    }

    public List<short> LineIndices;
    public Dictionary<PropertyType, Vector3[]> Properties;

    public CGridMesh(string path) : base(path) {
      LineIndices = new List<short>();
      Properties = new Dictionary<PropertyType, Vector3[]>();
    }

    public CGridMesh() : base() {
      LineIndices = new List<short>();
      Properties = new Dictionary<PropertyType, Vector3[]>();
    }

    public CGridMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.Grid) 
        : base (vertices, indices, figureType) {
      LineIndices = new List<short>();
      Properties = new Dictionary<PropertyType, Vector3[]>();
    }

    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }

    public void AddProperty(PropertyType type, Vector3[] values) {
      int vertexCountInOneGrid = 8;
      if (!Properties.ContainsKey(type) && Vertices.Count == values.Length * vertexCountInOneGrid) {
        Properties.Add(type, values);
      }
    }

    public void SetProperty(PropertyType type) {
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
