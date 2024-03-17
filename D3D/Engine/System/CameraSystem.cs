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
      }
    }

    private static void ChangePitch(CCamera camera, float pitch) {
      camera.IamCamera.Pitch += pitch;
      camera.IamEntity.RemoveComponent<CPitch>();
    }

    private static void ChangeYaw(CCamera camera, float yaw) {
      camera.IamCamera.Yaw += yaw;
      camera.IamEntity.RemoveComponent<CYaw>();
    }

    private static void ShiftUp(CCamera component) {
      component.IamCamera.Position += 0.05f * component.IamCamera.Up;
      component.IamEntity.RemoveComponent<CUpCameraMoving>();
    }

    private static void ShiftDown(CCamera component) {
      component.IamCamera.Position -= 0.05f * component.IamCamera.Up;
      component.IamEntity.RemoveComponent<CDownCameraMoving>();
    }

    private static void ShiftLeft(CCamera component) {
      component.IamCamera.Position += 0.05f * component.IamCamera.Right;
      component.IamEntity.RemoveComponent<CLeftCameraMoving>();
    }

    private static void ShiftRight(CCamera component) {
      component.IamCamera.Position -= 0.05f * component.IamCamera.Right;
      component.IamEntity.RemoveComponent<CRightCameraMoving>();
    }

    private static void ShiftFwd(CCamera component) {
      component.IamCamera.Position += 0.05f * component.IamCamera.Front;
      component.IamEntity.RemoveComponent<CFwdCameraMoving>();
    }

    private static void ShiftBack(CCamera component) {
      component.IamCamera.Position -= 0.05f * component.IamCamera.Front;
      component.IamEntity.RemoveComponent<CBackCameraMoving>();
    }
  }
}
