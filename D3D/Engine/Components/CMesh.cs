using System.Collections.Generic;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices;
    public List<short> FullIndices;

    public int XCount = -1;
    public int YCount = -1;
    public int ZCount = -1;

    public int CurrentXCount = -1;
    public int CurrentYCount = -1;
    public int CurrentZCount = -1;

    public CMesh(string path) {
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);
    }

    public CMesh() {
      Vertices = new List<VsBuffer>();
      Indices = new List<short>();
      FullIndices = new List<short>();
    }

    public CMesh(List<VsBuffer> vertices, List<short> indices) {
      Vertices = vertices;
      Indices = indices;
      FullIndices = new List<short>();
    }
  }
}
