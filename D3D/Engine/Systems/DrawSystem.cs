namespace D3D {
  public class DrawSystem : BaseSystem<CMesh> {
    new public static void Update() {
      foreach (var mesh in Components) {
        DrawMesh(mesh);
      }
    }

    private static void DrawMesh(CMesh mesh) {
      var lights = mesh.IamEntity.GetComponent<CLight>();
      var positions = mesh.IamEntity.GetComponent<CWorldPositions>();
      var camera = mesh.IamEntity.GetComponent<CCamera>();
      var transform = mesh.IamEntity.GetComponent<CTransform>();

      var vertices = mesh.Vertices.ToArray();
      var indices = mesh.Indices.ToArray();
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

      renderer.SetLightConstantBuffer(light);
      renderer.SetVerticesBuffer(vertices);
      renderer.SetIndicesBuffer(indices);


      for (int i = 0; i < positions.IamWorldMatrices.Count; ++i) {
        matr.world = positions.IamWorldMatrices[i];
        matr.world.Transpose();

        renderer.SetMvpConstantBuffer(matr);
        renderer.Draw(indices.Length);
      }
    }
  }
}
