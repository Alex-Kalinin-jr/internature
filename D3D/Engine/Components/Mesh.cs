using Assimp;

namespace D3D {
  public class CMesh : Component {
    public Mesh IamMesh;

    public CMesh(string path) {
      IamMesh = new Mesh(path);
    }

    public void Update() {
      var lights = IamEntity.GetComponent<CLight>();
      var positions = IamEntity.GetComponent<CWorldPositions>();
      var camera = IamEntity.GetComponent<CCamera>();
      var transform = IamEntity.GetComponent<CTransform>();

      if (lights != null) {
        PsLightConstantBuffer[] light = lights.IamLightData.ToArray();
        for (int i = 0; i < light.Length; i++) {
          light[i].ViewPos = camera.IamCamera.Position;
        }
      }
      var vertices = IamMesh.Vertices.ToArray();
      var indices = IamMesh.Indices.ToArray();
      var matr = transform.IamTransform;
      var tmp = new VsMvpConstantBuffer();
      tmp.view = camera.IamCamera.GetViewMatrix();
      tmp.view.Transpose();
      tmp.projection = camera.IamCamera.GetProjectionMatrix();
      tmp.projection.Transpose();

      // here // here
      for (int i = 0; i< positions.IamWorldMatrices.Count; i++) {
        tmp.world = 
      }
    }
  }
}
