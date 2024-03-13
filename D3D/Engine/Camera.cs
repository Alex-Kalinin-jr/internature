using SharpDX;
using SharpDX.Mathematics.Interop;
using System;

namespace D3D {
  public class Camera {
    public Vector3 Position { get; set; }
    public float AspectRatio { private get; set; }
    public Vector3 Front => _front;
    public Vector3 Up => _up;
    public Vector3 Right => _right;
    public float Pitch {
      get => MathUtil.RadiansToDegrees(_pitch);
      set {
        var angle = MathUtil.Clamp(value, -89f, 89f);
        _pitch = MathUtil.DegreesToRadians(angle);
        UpdateVectors();
      }
    }

    public float Yaw {
      get => MathUtil.RadiansToDegrees(_yaw);
      set {
        _yaw = MathUtil.DegreesToRadians(value);
        UpdateVectors();
      }
    }

    public float Fov {
      get => MathUtil.RadiansToDegrees(_fov);
      set {
        var angle = MathUtil.Clamp(value, 1f, 90f);
        _fov = MathUtil.DegreesToRadians(angle);
      }
    }

    private Vector3 _front = Vector3.UnitZ;
    private Vector3 _up = Vector3.UnitY;
    private Vector3 _right = Vector3.UnitX;
    private float _pitch;
    private float _yaw = -MathUtil.PiOverTwo;
    private float _fov = MathUtil.PiOverTwo;

    public Camera(Vector3 position, float aspectRatio) {
      Position = position;
      AspectRatio = aspectRatio;
    }

    public RawMatrix GetViewMatrix() {
      UpdateVectors();
      return Matrix.LookAtLH(Position, Position + _front, _up);
    }

    public RawMatrix GetProjectionMatrix() {
      UpdateVectors();
      return Matrix.PerspectiveFovLH(_fov, AspectRatio, 0.1f, 1000f);
    }

    private void UpdateVectors() {
      _front.X = (float)(Math.Cos(_pitch) * Math.Cos(_yaw));
      _front.Y = (float)Math.Sin(_pitch);
      _front.Z = (float)(Math.Cos(_pitch) * Math.Sin(_yaw));

      _front = Vector3.Normalize(_front);
      _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
      _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
  }
}