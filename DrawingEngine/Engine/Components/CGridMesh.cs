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

    public void SetProperty(string type) {
      if (Properties.ContainsKey(type)) {
        int vertexCountInOneGrid = 8;
        var prop = Properties[type];
        int counter = 0;
        System.Numerics.Vector3 botCol;
        System.Numerics.Vector3 topCol;
        (botCol, topCol) = ColorSystem.GetColors();
        for (int i = 0; i < Vertices.Count; i += vertexCountInOneGrid) {
          for (int j = 0; j < vertexCountInOneGrid; ++j) {

            var val = prop[counter];
            var r = botCol.X + (topCol.X - botCol.X) * val;
            var g = botCol.Y + (topCol.Y - botCol.Y) * val;
            var b = botCol.Z + (topCol.Z - botCol.Z) * val;

            var v = Vertices[i + j];
            v.Color = new Vector3(r, g, b);
            Vertices[i + j] = v;
          }
          ++counter;
        }
      }
    }
  }
}
