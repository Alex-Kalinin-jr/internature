using SharpDX;
using System;

namespace D3D {
  public class CameraSystem : BaseSystem<CCamera> {
    // Override the Update method of the base class to implement camera-specific update logic
    new public static void Update() {
      // Iterate over all CCamera components registered with the system
      foreach (var component in Components) {
        // Check if the camera entity has a CPitch component attached
        var pitch = component.EntityObj.GetComponent<CPitch>();
        if (pitch != null) {
          // Adjust the camera pitch based on the CPitch component value and remove the CPitch component
          ChangePitch(component, pitch.Pitch);
        }

        // Check if the camera entity has a CYaw component attached
        var yaw = component.EntityObj.GetComponent<CYaw>();
        if (yaw != null) {
          // Adjust the camera yaw based on the CYaw component value and remove the CYaw component
          ChangeYaw(component, yaw.Yaw);
        }

        // Update camera data (e.g., direction vectors) based on current pitch and yaw
        UpdateData(component);
      }
    }

    public static void ChangePitch(CCamera camera, float pitch) {
      // Clamp the pitch angle to avoid gimbal lock issues
      var angle = MathUtil.Clamp(pitch, -89f, 89f);
      // Update the camera pitch angle in radians and remove the CPitch component
      camera.Pitch += MathUtil.DegreesToRadians(angle);
      camera.EntityObj.RemoveComponent<CPitch>();
    }

    // Method to adjust the yaw of the camera
    public static void ChangeYaw(CCamera camera, float yaw) {
      // Update the camera yaw angle in radians and remove the CYaw component
      camera.Yaw += MathUtil.DegreesToRadians(yaw);
      camera.EntityObj.RemoveComponent<CYaw>();
    }

    // Methods to shift the camera position in different directions
    public static void ShiftUp() {
      foreach (var component in Components) {
        component.Position += 0.1f * component.Up;
      }
    }

    public static void ShiftDown() {
      foreach (var component in Components) {
        component.Position -= 0.1f * component.Up;
      }
    }

    public static void ShiftLeft() {
      foreach (var component in Components) {
        component.Position += 0.1f * component.Right;
      }
    }

    public static void ShiftRight() {
      foreach (var component in Components) {
        component.Position -= 0.1f * component.Right;
      }
    }

    public static void ShiftFwd() {
      foreach (var component in Components) {
        component.Position += 0.1f * component.Front;
      }
    }

    public static void ShiftBack() {
      foreach (var component in Components) {
        component.Position -= 0.1f * component.Front;
      }
    }

    // Method to update camera data such as front, right, and up vectors based on current pitch and yaw
    private static void UpdateData(CCamera camera) {
      // Calculate the new front vector based on the current pitch and yaw angles
      camera.Front.X = (float)(Math.Cos(camera.Pitch) * Math.Cos(camera.Yaw));
      camera.Front.Y = (float)Math.Sin(camera.Pitch);
      camera.Front.Z = (float)(Math.Cos(camera.Pitch) * Math.Sin(camera.Yaw));

      // Normalize the front vector to ensure unit length
      camera.Front = Vector3.Normalize(camera.Front);

      // Calculate the new right vector based on the updated front vector and the world up vector
      camera.Right = Vector3.Normalize(Vector3.Cross(camera.Front, Vector3.UnitY));

      // Calculate the new up vector based on the updated right and front vectors
      camera.Up = Vector3.Normalize(Vector3.Cross(camera.Right, camera.Front));
    }
  }
}