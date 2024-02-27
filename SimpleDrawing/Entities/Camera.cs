
namespace SimpleDrawing.Entities {

  public class Camera {
    public OpenTK.Mathematics.Vector3 Position { get; set; }
    public float AspectRatio { private get; set; }
    public OpenTK.Mathematics.Vector3 Front => _front;
    public OpenTK.Mathematics.Vector3 Up => _up;
    public OpenTK.Mathematics.Vector3 Right => _right;
    public float Pitch {
      get => OpenTK.Mathematics.MathHelper.RadiansToDegrees(_pitch);
      set {
        var angle = OpenTK.Mathematics.MathHelper.Clamp(value, -89f, 89f);
        _pitch = OpenTK.Mathematics.MathHelper.DegreesToRadians(angle);
        UpdateVectors();
      }
    }
    public float Yaw {
      get => OpenTK.Mathematics.MathHelper.RadiansToDegrees(_yaw);
      set {
        _yaw = OpenTK.Mathematics.MathHelper.DegreesToRadians(value);
        UpdateVectors();
      }
    }
    public float Fov {
      get => OpenTK.Mathematics.MathHelper.RadiansToDegrees(_fov);
      set {
        var angle = OpenTK.Mathematics.MathHelper.Clamp(value, 1f, 90f);
        _fov = OpenTK.Mathematics.MathHelper.DegreesToRadians(angle);
      }
    }


    private OpenTK.Mathematics.Vector3 _front = -OpenTK.Mathematics.Vector3.UnitZ;
    private OpenTK.Mathematics.Vector3 _up = OpenTK.Mathematics.Vector3.UnitY;
    private OpenTK.Mathematics.Vector3 _right = OpenTK.Mathematics.Vector3.UnitX;
    private float _pitch;
    private float _yaw = -OpenTK.Mathematics.MathHelper.PiOver2;
    private float _fov = OpenTK.Mathematics.MathHelper.PiOver2;


    public Camera(OpenTK.Mathematics.Vector3 position, float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio;
    }


    public OpenTK.Mathematics.Matrix4 GetViewMatrix() {
      return OpenTK.Mathematics.Matrix4.LookAt(Position, Position + _front, _up);
    }

    public OpenTK.Mathematics.Matrix4 GetProjectionMatrix() {
      return OpenTK.Mathematics.Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
    }

    private void UpdateVectors() {
      _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
      _front.Y = MathF.Sin(_pitch);
      _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

      _front = OpenTK.Mathematics.Vector3.Normalize(_front);
      _right = OpenTK.Mathematics.Vector3.Normalize(OpenTK.Mathematics.Vector3.Cross(_front, OpenTK.Mathematics.Vector3.UnitY));
      _up = OpenTK.Mathematics.Vector3.Normalize(OpenTK.Mathematics.Vector3.Cross(_right, _front));
    }
  }
}
