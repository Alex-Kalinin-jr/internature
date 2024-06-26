﻿using SharpDX;
using System;

namespace D3D {
  /// <summary>
  /// Class responsible for managing camera components.
  /// </summary>
  public class CameraSystem : BaseSystem<CCamera> {
    private static float _cameraSpeed = 0.1f;
    /// <summary>
    /// Overrides the Update method of the base class to implement camera-specific update logic.
    /// </summary>
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

    /// <summary>
    /// Changes the pitch (vertical rotation) of the camera.
    /// </summary>
    /// <param name="camera">The camera to adjust.</param>
    /// <param name="pitch">The pitch angle to apply.</param>
    public static void ChangePitch(CCamera camera, float pitch) {
      // Clamp the pitch angle to avoid gimbal lock issues
      var angle = MathUtil.Clamp(pitch, -89f, 89f); // assuming that we can not rotate our head (eyes) backwards over top or down
      // Update the camera pitch angle in radians and remove the CPitch component
      camera.Pitch += MathUtil.DegreesToRadians(angle);
      camera.EntityObj.RemoveComponent<CPitch>();
    }

    /// <summary>
    /// Changes the yaw (horizontal rotation) of the camera.
    /// </summary>
    /// <param name="camera">The camera to adjust.</param>
    /// <param name="yaw">The yaw angle to apply.</param>
    public static void ChangeYaw(CCamera camera, float yaw) {
      // Update the camera yaw angle in radians and remove the CYaw component
      camera.Yaw += MathUtil.DegreesToRadians(yaw);
      camera.EntityObj.RemoveComponent<CYaw>();
    }

    // Methods to shift the camera position in different directions

    /// <summary>
    /// Moves the camera upwards.
    /// </summary>
    public static void ShiftUp() {
      foreach (var component in Components) {
        component.Position += _cameraSpeed * component.Up;
      }
    }

    /// <summary>
    /// Moves the camera downwards.
    /// </summary>
    public static void ShiftDown() {
      foreach (var component in Components) {
        component.Position -= _cameraSpeed * component.Up;
      }
    }

    /// <summary>
    /// Moves the camera to the left.
    /// </summary>
    public static void ShiftLeft() {
      foreach (var component in Components) {
        component.Position += _cameraSpeed * component.Right;
      }
    }

    /// <summary>
    /// Moves the camera to the right.
    /// </summary>
    public static void ShiftRight() {
      foreach (var component in Components) {
        component.Position -= _cameraSpeed * component.Right;
      }
    }

    /// <summary>
    /// Moves the camera forwards.
    /// </summary>
    public static void ShiftFwd() {
      foreach (var component in Components) {
        component.Position += _cameraSpeed * component.Front;
      }
    }

    /// <summary>
    /// Moves the camera backwards.
    /// </summary>
    public static void ShiftBack() {
      foreach (var component in Components) {
        component.Position -= _cameraSpeed * component.Front;
      }
    }

    /// <summary>
    /// Updates camera data such as front, right, and up vectors based on current pitch and yaw.
    /// </summary>
    /// <param name="camera">The camera to update.</param>
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