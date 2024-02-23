using OpenTK.Mathematics;

namespace SimpleDrawing.Entities {
  public struct DirectionalLight {
    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
    public Vector3 Direction { get; set; }
  }

  public struct PointLight { 
    public Vector3 Position { get; set; }

    public Vector3 Color { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set;}

    public float Constant {  get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }
  }


}