namespace D3D {
  public class CCamera : Component {
    public Camera IamCamera;

    public CCamera() {
      CameraSystem.Register(this);
    }
  }
}
