using SharpDX;

namespace D3D {
  public class LightSystem : BaseSystem<CLight> {
    // Override the Update method of the base class to implement specific update logic for lights
    new public static void Update() {
      foreach (var component in Components) {
        SetViewPosition(component); // Update the view position for each light
      }
    }
    // Method to change the color of all lights
    public static void ChangeColor(Vector4 color) {
      foreach (var component in Components) {
        for (int i = 0; i < component.LightDataObj.Count; i++) {
          var tmp = component.LightDataObj[i];
          tmp.Color = color;
          component.LightDataObj[i] = tmp;
        }
      }
    }
    // Method to change the position of all lights
    public static void ChangePosition(Vector3 position) {
      foreach (var component in Components) {
        for (int i = 0; i < component.LightDataObj.Count; i++) {
          var tmp = component.LightDataObj[i];
          tmp.Position = position;
          component.LightDataObj[i] = tmp;
        }

      }
    }
    // Private method to set the view position for each light based on the camera's position
    private static void SetViewPosition(CLight component) {
      var camera = component.EntityObj.GetComponent<CCamera>(); // Get the camera associated with the light
      var light = component.LightDataObj; // Get the light data for the component
      for (int i = 0; i < light.Count; ++i) {
        var buff = light[i];
        buff.ViewPos = camera.Position;
        light[i] = buff;
      }
    }
  }
}
