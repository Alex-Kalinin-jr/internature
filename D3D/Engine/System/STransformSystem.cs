
using SharpDX.Mathematics.Interop;
using SharpDX;

namespace D3D {
  class TransformSystem : BaseSystem<CTransform> {
    new public static void Update() {
      foreach (var component in Components) {
        component.TransformObj.view = GetViewMatrix();
        component.TransformObj.projection = GetProjectionMatrix();
      }
    }

    private static RawMatrix GetViewMatrix() {
      var camera = Components[0].EntityObj.GetComponent<CCamera>();
      var matrix = Matrix.LookAtLH(camera.Position, camera.Position + camera.Front, camera.Up);
      matrix.Transpose();
      return matrix;
    }

    private static RawMatrix GetProjectionMatrix() {
      var camera = Components[0].EntityObj.GetComponent<CCamera>();
      var matrix = Matrix.PerspectiveFovLH(camera.Fov, camera.AspectRatio, 0.1f, 1000f);
      matrix.Transpose();
      return matrix;
    }

  }
}
