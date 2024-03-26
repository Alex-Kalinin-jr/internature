using SharpDX;

namespace D3D {
  /// <summary>
  /// Represents a camera component used for viewing scenes.
  /// </summary>
  public class CCamera : Component {
    /// <summary>
    /// The position of the camera in 3D space.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// The aspect ratio of the camera (width / height).
    /// </summary>
    public float AspectRatio;

    /// <summary>
    /// The forward direction of the camera.
    /// </summary>
    public Vector3 Front = Vector3.UnitZ; // Forward direction

    /// <summary>
    /// The upward direction of the camera.
    /// </summary>
    public Vector3 Up = Vector3.UnitY;

    /// <summary>
    /// The rightward direction of the camera.
    /// </summary>
    public Vector3 Right = Vector3.UnitX;
    /// <summary>
    /// The pitch angle of the camera (rotation around the x-axis).
    /// </summary>
    public float Pitch = 0.0f;

    /// <summary>
    /// The yaw angle of the camera (rotation around the y-axis).
    /// </summary>
    public float Yaw = 0;

    /// <summary>
    /// The field of view angle of the camera.
    /// </summary>
    public float Fov = MathUtil.PiOverTwo;

    /// <summary>
    /// Initializes a new instance of the <see cref="CCamera"/> class with the specified position and aspect ratio.
    /// </summary>
    /// <param name="position">The initial position of the camera.</param>
    /// <param name="aspectRatio">The aspect ratio of the camera.</param>
    public CCamera(Vector3 position, float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio;
      CameraSystem.Register(this);
    }
  }
}
