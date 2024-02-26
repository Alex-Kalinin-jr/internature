using OpenTK.Mathematics;

public struct Material {
  public Vector3 Ambient { get; set; }
  public Vector3 Diffuse { get; set; }
  public Vector3 Specular { get; set; }

  public float Shiness { get; set; }
  public Material() {
    Ambient = new Vector3(1.0f, 0.5f, 0.0f);
    Diffuse = new Vector3(1.0f, 0.5f, 0.31f);
    Specular = new Vector3(0.5f, 0.5f, 0.5f);
    Shiness = 0.32f;
  }
}