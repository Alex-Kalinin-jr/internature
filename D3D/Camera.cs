using System;
using SharpDX;


namespace D3D {
  public class Camera {
    public Matrix World { get; set; }
    public Matrix View { get; set; }
    public Matrix Projection { get; set; }


    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public Vector3 Up { get; set; }
    public Vector3 Left { get; set; }


    public Camera() {

      Position = new Vector3(0.0f, 0.9f, -2.6f);
      Target = new Vector3(0.0f, 0.0f, 0.0f);
      Up = new Vector3(0.0f, 1.0f, 0.0f);
      Left = new Vector3(0.0f, 0.0f, 1.0f);

      World = Matrix.Identity;
      View = Matrix.Identity;
      Projection = Matrix.Identity;

      Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, 1.2f, 1.0f, 1000.0f);

    }


    public void SetLens(float Fov, float Aspect, float Zn, float Zf) {
      Projection = Matrix.PerspectiveFovLH(Fov, Aspect, Zn, Zf);
    }


    public void Update() {
      View = Matrix.LookAtLH(Position, Target, Up);
    }

    public void RotateX(float angle) {
      // Rotate the camera around the global X-axis
      Matrix rotation = Matrix.RotationX(angle);
      RotateCamera(rotation);
    }

    public void RotateY(float angle) {
      // Rotate the camera around the global Y-axis
      Matrix rotation = Matrix.RotationY(angle);
      RotateCamera(rotation);
    }

    public void RotateZ(float angle) {
      // Rotate the camera around the global Z-axis
      Matrix rotation = Matrix.RotationZ(angle);
      RotateCamera(rotation);
    }

    private void RotateCamera(Matrix rotation) {
      // Apply the rotation to the camera's position, target, and up vectors
      Position = Vector3.TransformCoordinate(Position, rotation);
      Target = Vector3.TransformCoordinate(Target, rotation);
      Up = Vector3.TransformCoordinate(Up, rotation);
      Left = Vector3.Cross(Up, Target); // Update the left vector
      Update(); // Update the view matrix
    }
  }
}
