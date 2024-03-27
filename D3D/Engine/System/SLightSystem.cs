using SharpDX;

namespace D3D {
  /// <summary>
  /// Class representing the light system.
  /// </summary>
  public class LightSystem : BaseSystem<CLight> {
    /// <summary>
    /// Method to update the light system.
    /// </summary>
    new public static void Update() {
      foreach (var component in Components) {
        SetViewPosition(component); // Update the view position for each light
      }
    }

    /// <summary>
    /// Method to change the color of all lights.
    /// </summary>
    /// <param name="color">The new color for the lights.</param>
    public static void ChangeColor(Vector4 color) {
      foreach (var component in Components) {
        for (int i = 0; i < component.LightDataObj.Count; i++) {
          var tmp = component.LightDataObj[i];
          tmp.Color = color;
          component.LightDataObj[i] = tmp;
        }
      }
    }

    /// <summary>
    /// Method to change the position of all lights.
    /// </summary>
    /// <param name="position">The new position for the lights.</param>
    public static void ChangePosition(Vector3 position) {
      foreach (var component in Components) {
        for (int i = 0; i < component.LightDataObj.Count; i++) {
          var tmp = component.LightDataObj[i];
          tmp.Position = position;
          component.LightDataObj[i] = tmp;
        }
      }
    }

    /// <summary>
    /// Method to set the view position for a light component.
    /// </summary>
    /// <param name="component">The light component.</param>
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
