namespace SimpleDrawing.Model {
  public class MaterialColor : Color {

    public float Shiness;

    public MaterialColor() {
      Shiness = 0.32f; // just an example (should be from 0 to 1)
    }

    public static MaterialColor CreateMaterial(string name) {
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
        return new MaterialColor();
      }
    }
// just an example
    public static MaterialColor CreateEmerald() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.0215f, 0.1745f, 0.0215f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.07568f, 0.61424f, 0.07568f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.633f, 0.727811f, 0.633f);
      buff.Shiness = 0.6f;
      return buff;
    }
// just an example
    public static MaterialColor CreateObsidian() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.05375f, 0.05f, 0.06625f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.18275f, 0.17f, 0.22525f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.332741f, 0.328634f, 0.346435f);
      buff.Shiness = 0.3f;
      return buff;
    }
// just an example
    public static MaterialColor CreateChrome() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.25f, 0.25f, 0.25f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.4f, 0.4f, 0.4f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.774597f, 0.774597f, 0.774597f);
      buff.Shiness = 0.6f;
      return buff;
    }
// just an example
    public static MaterialColor CreateBlackRubber() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.02f, 0.02f, 0.02f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.01f, 0.01f, 0.01f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.4f, 0.4f, 0.4f);
      buff.Shiness = 0.078125f;
      return buff;
    }
// just an example
    public static MaterialColor CreateGreenPlastic() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.0f, 0.0f, 0.0f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.1f, 0.35f, 0.1f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.45f, 0.55f, 0.45f);
      buff.Shiness = 0.25f;
      return buff;
    }
// just an example
    public static MaterialColor CreateBronze() {
      MaterialColor buff = new MaterialColor();
      buff.Ambient = new OpenTK.Mathematics.Vector3(0.2125f, 0.1275f, 0.054f);
      buff.Diffuse = new OpenTK.Mathematics.Vector3(0.714f, 0.4284f, 0.18144f);
      buff.Specular = new OpenTK.Mathematics.Vector3(0.393548f, 0.271906f, 0.166721f);
      buff.Shiness = 0.2f;
      return buff;
    }

  }
}

