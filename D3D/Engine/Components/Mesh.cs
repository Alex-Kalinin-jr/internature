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

      var vertices = IamMesh.Vertices.ToArray();
      var indices = IamMesh.Indices.ToArray();
      var matr = transform.IamTransform;
      var renderer = Renderer.GetRenderer();

      PsLightConstantBuffer[] light = lights.IamLightData.ToArray();
      for (int i = 0; i < light.Length; ++i) {
        light[i].ViewPos = camera.IamCamera.Position;
      }


      matr.view = camera.IamCamera.GetViewMatrix();
      matr.view.Transpose();
      matr.projection = camera.IamCamera.GetProjectionMatrix();
      matr.projection.Transpose();

      for (int i = 0; i < positions.IamWorldMatrices.Count; ++i) {
        matr.world = positions.IamWorldMatrices[i];
        matr.world.Transpose();
        renderer.RenderCallback(light, vertices, indices, matr);
      }

      renderer.Present();
    }
  }
}
