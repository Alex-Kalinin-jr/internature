using Assimp;
using SharpDX;
using System.Diagnostics;
using System.Drawing;
using System;

namespace D3D {
  public class CameraSystem : BaseSystem<CCamera> {
    new public static void Update() {
      foreach (var component in Components) {

        var pitch = component.IamEntity.GetComponent<CPitch>();
        if (pitch != null) {
          ChangePitch(component, pitch.Pitch);
        }

        var yaw = component.IamEntity.GetComponent<CYaw>();
        if (yaw != null) {
          ChangeYaw(component, yaw.Yaw);
        }

        if (component.IamEntity.GetComponent<CUpCameraMoving>() != null) {
          ShiftUp(component);
        }

        if (component.IamEntity.GetComponent<CDownCameraMoving>() != null) {
          ShiftDown(component);
        }

        if (component.IamEntity.GetComponent<CLeftCameraMoving>() != null) {
          ShiftLeft(component);
        }

        if (component.IamEntity.GetComponent<CRightCameraMoving>() != null) {
          ShiftRight(component);
        }

        if (component.IamEntity.GetComponent<CFwdCameraMoving>() != null) {
          ShiftFwd(component);
        }

        if (component.IamEntity.GetComponent<CBackCameraMoving>() != null) {
          ShiftBack(component);
        }
        UpdateData(component);
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


    private static void ChangePitch(CCamera camera, float pitch) {
      var angle = MathUtil.Clamp(pitch, -89f, 89f);
      camera.Pitch += MathUtil.DegreesToRadians(angle);
      camera.IamEntity.RemoveComponent<CPitch>();
    }

    private static void ChangeYaw(CCamera camera, float yaw) {
      camera.Yaw += MathUtil.DegreesToRadians(yaw);
      camera.IamEntity.RemoveComponent<CYaw>();
    }

    private static void ShiftUp(CCamera component) {
      component.Position += 0.05f * component.Up;
      component.IamEntity.RemoveComponent<CUpCameraMoving>();
    }

    private static void ShiftDown(CCamera component) {
      component.Position -= 0.05f * component.Up;
      component.IamEntity.RemoveComponent<CDownCameraMoving>();
    }

    private static void ShiftLeft(CCamera component) {
      component.Position += 0.05f * component.Right;
      component.IamEntity.RemoveComponent<CLeftCameraMoving>();
    }

    private static void ShiftRight(CCamera component) {
      component.Position -= 0.05f * component.Right;
      component.IamEntity.RemoveComponent<CRightCameraMoving>();
    }

    private static void ShiftFwd(CCamera component) {
      component.Position += 0.05f * component.Front;
      component.IamEntity.RemoveComponent<CFwdCameraMoving>();
    }

    private static void ShiftBack(CCamera component) {
      component.Position -= 0.05f * component.Front;
      component.IamEntity.RemoveComponent<CBackCameraMoving>();
    }
  }
}
