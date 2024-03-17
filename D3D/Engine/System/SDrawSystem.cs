namespace D3D {
  public class DrawSystem : BaseSystem<CFigure> {
    new public static void Update() {
      foreach (var figure in Components) {
        DrawFigure(figure);
      }
    }

    private static void DrawFigure(CFigure figure) {

      var vertices = figure.IamMesh.Vertices.ToArray();
      var indices = figure.IamMesh.Indices.ToArray();
      var matr = figure.IamTransform.IamTransform;
      var lights = figure.IamEntity.GetComponent<CLight>();
      PsLightConstantBuffer[] light = lights.IamLightData.ToArray();


      var renderer = Renderer.GetRenderer();
      renderer.SetLightConstantBuffer(ref light);
      renderer.SetVerticesBuffer(ref vertices);
      renderer.SetIndicesBuffer(ref indices);
      renderer.SetMvpConstantBuffer(ref matr);
      renderer.Draw(indices.Length);
    }
  }
}
