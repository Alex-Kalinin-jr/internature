using System.Collections.Generic;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices;

    public CMesh(string path) {
      (Vertices, Indices) = Generator.GenerateMesh(path); // testing initializing
    }
  }
}
