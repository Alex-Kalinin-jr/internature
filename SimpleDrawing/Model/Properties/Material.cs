
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

  public static Material CreateMaterial(string name) {
    if (name == "emerald") {
      return CreateEmerald();
    } else if (name == "obsidian") {
      return CreateObsidian();
    } else if (name == "chrome") {
      return CreateChrome();
    } else if (name == "blackRubber") {
      return CreateBlackRubber();
    } else if (name == "greenPlastic") {
      return CreateGreenPlastic();
    } else if (name == "bronze") {
      return CreateBronze();
    } else {
      return new Material();
    }
  }
  public static Material CreateEmerald() {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.0215f, 0.1745f, 0.0215f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.07568f, 0.61424f, 0.07568f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.633f, 0.727811f, 0.633f);
    buff.Shiness = 0.6f;
    return buff;
  }

  public static Material CreateObsidian() {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.05375f, 0.05f, 0.06625f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.18275f, 0.17f, 0.22525f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.332741f, 0.328634f, 0.346435f);
    buff.Shiness = 0.3f;
    return buff;
  }

  public static Material CreateChrome() {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.25f, 0.25f, 0.25f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.4f, 0.4f, 0.4f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.774597f, 0.774597f, 0.774597f);
    buff.Shiness = 0.6f;
    return buff;
  }

  public static Material CreateBlackRubber() {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.02f, 0.02f, 0.02f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.01f, 0.01f, 0.01f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.4f, 0.4f, 0.4f);
    buff.Shiness = 0.078125f;
    return buff;
  }

  public static Material CreateGreenPlastic  () {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.0f, 0.0f, 0.0f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.1f, 0.35f, 0.1f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.45f, 0.55f, 0.45f);
    buff.Shiness = 0.25f;
    return buff;
  }

  public static Material CreateBronze() {
    Material buff = new Material();
    buff.Ambient = new OpenTK.Mathematics.Vector3(0.2125f, 0.1275f, 0.054f);
    buff.Diffuse = new OpenTK.Mathematics.Vector3(0.714f, 0.4284f, 0.18144f);
    buff.Specular = new OpenTK.Mathematics.Vector3(0.393548f, 0.271906f, 0.166721f);
    buff.Shiness = 0.2f;
    return buff;
  }

}
