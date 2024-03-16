namespace D3D {
  public class CMesh : Component {
    public Mesh IamMesh;

    public CMesh(string path) {
      IamMesh = new Mesh(path);
    }
  }
}
