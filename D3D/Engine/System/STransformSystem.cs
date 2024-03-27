using SharpDX.Mathematics.Interop;
using SharpDX;

namespace D3D {
  /// <summary>
  /// Class representing the transform system.
  /// </summary>
  class TransformSystem : BaseSystem<CTransform> {
    /// <summary>
    /// Method to update the transform system.
    /// </summary>
    new public static void Update() {
      foreach (var component in Components) {
        // Update the view and projection matrices for each transform component
        component.TransformObj.view = GetViewMatrix();
        component.TransformObj.projection = GetProjectionMatrix();
      }
    }

    /// <summary>
    /// Method to calculate the view matrix.
    /// </summary>
    /// <returns>The calculated view matrix.</returns>
    private static RawMatrix GetViewMatrix() {
      var camera = Components[0].EntityObj.GetComponent<CCamera>(); // Get the camera associated with the first transform component
      var matrix = Matrix.LookAtLH(camera.Position, camera.Position + camera.Front, camera.Up); // Calculate the view matrix
      matrix.Transpose(); // Transpose the matrix
      return matrix;
    }

    /// <summary>
    /// Method to calculate the projection matrix.
    /// </summary>
    /// <returns>The calculated projection matrix.</returns>
    private static RawMatrix GetProjectionMatrix() {
      var frontFace = 0.1f;
      var backFace = 1000f;
      var camera = Components[0].EntityObj.GetComponent<CCamera>(); // Get the camera associated with the first transform component
      var matrix = Matrix.PerspectiveFovLH(camera.Fov, camera.AspectRatio, frontFace, backFace); // Calculate the projection matrix
      matrix.Transpose(); // Transpose the matrix
      return matrix;
    }
  }
}
