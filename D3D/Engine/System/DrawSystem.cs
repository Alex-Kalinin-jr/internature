namespace D3D {
  public class DrawSystem : BaseSystem<CFigure> {
    new public static void Update() {
      foreach (var figure in Components) {
        DrawFigure(figure);
      }
    }

    private static void DrawFigure(CFigure figure) {
      var lights = figure.IamEntity.GetComponent<CLight>();
      var camera = figure.IamEntity.GetComponent<CCamera>();

      var vertices = figure.IamMesh.Vertices.ToArray();
      var indices = figure.IamMesh.Indices.ToArray();
      var matr = figure.IamTransform.IamTransform;
      var renderer = Renderer.GetRenderer();

      PsLightConstantBuffer[] light = lights.IamLightData.ToArray();
      for (int i = 0; i < light.Length; ++i) {
        light[i].ViewPos = camera.IamCamera.Position;
      }


      // to be set into camera component
      matr.view = camera.IamCamera.GetViewMatrix();
      matr.view.Transpose();
      matr.projection = camera.IamCamera.GetProjectionMatrix();
      matr.projection.Transpose();




      renderer.SetLightConstantBuffer(light);
      renderer.SetVerticesBuffer(vertices);
      renderer.SetIndicesBuffer(indices);

      renderer.SetMvpConstantBuffer(matr);
      renderer.Draw(indices.Length);
    }
  }
}
