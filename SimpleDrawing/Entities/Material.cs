
public struct Material {
  public OpenTK.Mathematics.Vector3 Ambient { get; set; }
  public OpenTK.Mathematics.Vector3 Diffuse { get; set; }
  public OpenTK.Mathematics.Vector3 Specular { get; set; }
  public float Shiness { get; set; }
  public Material() {
    Ambient = new OpenTK.Mathematics.Vector3(1.0f, 0.5f, 0.0f);
    Diffuse = new OpenTK.Mathematics.Vector3(1.0f, 0.5f, 0.31f);
    Specular = new OpenTK.Mathematics.Vector3(0.5f, 0.5f, 0.5f);
    Shiness = 0.32f;
  }
}