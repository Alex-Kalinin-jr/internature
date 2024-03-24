using SharpDX;

namespace D3D {
  public class CCamera : Component {
    public Vector3 Position;
    public float AspectRatio;

    public Vector3 Front = Vector3.UnitZ;
    public Vector3 Up = Vector3.UnitY;
    public Vector3 Right = Vector3.UnitX;

    public float Pitch = 0.0f;
    public float Yaw = MathUtil.PiOverTwo;
    public float Fov = MathUtil.PiOverTwo;

    public CCamera(Vector3 position, float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio;
      CameraSystem.Register(this);
    }
  }
}
