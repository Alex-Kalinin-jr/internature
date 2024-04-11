using SharpDX.Mathematics.Interop;
using SharpDX;
using System;

namespace D3D {
  /// <summary>
  /// Class representing the transform system.
  /// </summary>
  public class TransformSystem : BaseSystem<CTransform> {
    /// <summary>
    /// Method to update the transform system.
    /// </summary>
    /// 

    private static float _scaleFactor = 1;

    new public static void Update() {
      foreach (var component in Components) {
        // Update the view and projection matrices for each transform component
        component.TransformObj.view = GetViewMatrix();
        component.TransformObj.projection = GetProjectionMatrix();
      }
    }

    public static void ChangeScale(int delta) {
      var stepFactor = 0.01f;
      var bottomBorder = 0.1f;
      foreach (var figure in Components) {
        figure.TransformObj.world.Transpose();
        figure.TransformObj.world.M11 /= _scaleFactor;
        figure.TransformObj.world.M22 /= _scaleFactor;
        figure.TransformObj.world.M33 /= _scaleFactor;
      }

      _scaleFactor += (delta > 0) ? stepFactor : -stepFactor;
      _scaleFactor = Math.Max(_scaleFactor, bottomBorder);
      var newScale = _scaleFactor;

      foreach (var figure in Components) {
        figure.TransformObj.world.M11 *= _scaleFactor;
        figure.TransformObj.world.M22 *= _scaleFactor;
        figure.TransformObj.world.M33 *= _scaleFactor;
        figure.TransformObj.world.Transpose();
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


    /// <summary>
    /// Computes the testing model matrix.
    /// </summary>
    /// <param name="rotations">The rotation vector.</param>
    /// <param name="translations">The translation vector.</param>
    /// <returns>The computed model matrix.</returns>
    public static Matrix ComputeModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      buff.Transpose();
      return buff;
    }
  }
}
