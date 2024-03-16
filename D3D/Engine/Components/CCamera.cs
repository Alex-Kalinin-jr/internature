namespace D3D {
  public class CCamera : Component {
    public Camera IamCamera;

    public CCamera() {
      CameraSystem.Register(this);
    }

    public override void Update() {

      var pitch = IamEntity.GetComponent<CPitch>();
      if (pitch != null ) {
        IamCamera.Pitch += pitch.Pitch;
        IamEntity.RemoveComponent<CPitch>();
      }

      var yaw = IamEntity.GetComponent<CYaw>();
      if (yaw != null) {
        IamCamera.Yaw += yaw.Yaw;
        IamEntity.RemoveComponent<CYaw>();
      }

      if (IamEntity.GetComponent<CUpCameraMoving>() != null) {
        IamCamera.Position += 0.05f * IamCamera.Up;
        IamEntity.RemoveComponent<CUpCameraMoving>();
      }

      if (IamEntity.GetComponent<CDownCameraMoving>() != null) {
        IamCamera.Position -= 0.05f * IamCamera.Up;
        IamEntity.RemoveComponent<CDownCameraMoving>();
      }

      if (IamEntity.GetComponent<CLeftCameraMoving>() != null) {
        IamCamera.Position += 0.05f * IamCamera.Right;
        IamEntity.RemoveComponent<CLeftCameraMoving>();
      }

      if (IamEntity.GetComponent<CRightCameraMoving>() != null) {
        IamCamera.Position -= 0.05f * IamCamera.Right;
        IamEntity.RemoveComponent<CRightCameraMoving>();
      }

      if (IamEntity.GetComponent<CFwdCameraMoving>() != null) {
        IamCamera.Position += 0.05f * IamCamera.Front;
        IamEntity.RemoveComponent<CFwdCameraMoving>();
      }

      if (IamEntity.GetComponent<CBackCameraMoving>() != null) {
        IamCamera.Position -= 0.05f * IamCamera.Front;
        IamEntity.RemoveComponent<CBackCameraMoving>();
      }
    }
  }
}
