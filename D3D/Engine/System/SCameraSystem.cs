using SharpDX;
using System;

namespace D3D {
  public class CameraSystem : BaseSystem<CCamera> {
    new public static void Update() {
      foreach (var component in Components) {

        var pitch = component.EntityObj.GetComponent<CPitch>();
        if (pitch != null) {
          ChangePitch(component, pitch.Pitch);
        }

        var yaw = component.EntityObj.GetComponent<CYaw>();
        if (yaw != null) {
          ChangeYaw(component, yaw.Yaw);
        }
        UpdateData(component);
      }
    }


    public static void ChangePitch(CCamera camera, float pitch) {
      var angle = MathUtil.Clamp(pitch, -89f, 89f);
      camera.Pitch += MathUtil.DegreesToRadians(angle);
      camera.EntityObj.RemoveComponent<CPitch>();
    }

    public static void ChangeYaw(CCamera camera, float yaw) {
      camera.Yaw += MathUtil.DegreesToRadians(yaw);
      camera.EntityObj.RemoveComponent<CYaw>();
    }

    public static void ShiftUp() {
      foreach (var component in Components) {
        component.Position += 0.05f * component.Up;
      }
    }

    public static void ShiftDown() {
      foreach (var component in Components) {
        component.Position -= 0.05f * component.Up;
      }
    }

    public static void ShiftLeft( ) {
      foreach (var component in Components) {
        component.Position += 0.05f * component.Right;
      }
    }

    public static void ShiftRight() {
      foreach (var component in Components) {
        component.Position -= 0.05f * component.Right;
      }
    }

    public static void ShiftFwd() {
      foreach (var component in Components) {
        component.Position += 0.05f * component.Front;
      }
    }

    public static void ShiftBack() {
      foreach (var component in Components) {
        component.Position -= 0.05f * component.Front;
      }
    }

    private static void UpdateData(CCamera camera) {
      camera.Front.X = (float)(Math.Cos(camera.Pitch) * Math.Cos(camera.Yaw));
      camera.Front.Y = (float)Math.Sin(camera.Pitch);
      camera.Front.Z = (float)(Math.Cos(camera.Pitch) * Math.Sin(camera.Yaw));

      camera.Front = Vector3.Normalize(camera.Front);
      camera.Right = Vector3.Normalize(Vector3.Cross(camera.Front, Vector3.UnitY));
      camera.Up = Vector3.Normalize(Vector3.Cross(camera.Right, camera.Front));
    }
  }
}
