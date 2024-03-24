using SharpDX;

namespace D3D {
  public class CCamera : Component {
    // Public fields to hold camera properties
    public Vector3 Position; 
    public float AspectRatio; 

    // Default direction vectors for the camera orientation
    public Vector3 Front = Vector3.UnitZ; // Forward direction
    public Vector3 Up = Vector3.UnitY; // Upward direction
    public Vector3 Right = Vector3.UnitX; // Rightward direction

    // Euler angles to represent camera orientation
    public float Pitch = 0.0f; // Pitch angle (rotation around the x-axis)
    public float Yaw = MathUtil.PiOverTwo; // Yaw angle (rotation around the y-axis)
    public float Fov = MathUtil.PiOverTwo; // Field of view angle

    // Constructor for initializing camera with position and aspect ratio
    public CCamera(Vector3 position, float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio; 
      CameraSystem.Register(this);
    }
  }
}