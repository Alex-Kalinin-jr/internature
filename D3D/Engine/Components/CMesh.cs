using System.Collections.Generic;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices;

    public CMesh(string path) {
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path); // testing initializing
    }

    public CMesh() {
      Vertices = new List<VsBuffer>();
      Indices = new List<short>();
    }

    public CMesh(List<VsBuffer> vertices, List<short> indices) {
      Vertices = vertices;
      Indices = indices;
    }
  }
}
