using SharpDX;

namespace D3D {
  public class LightSystem : BaseSystem<CLight> {
    new public static void Update() {
      foreach (var component in Components) {
        SetViewPosition(component);
      }
    }

    public static void ChangeColor(Vector4 color) {
      foreach ( var component in Components) {
        for (int i = 0; i < component.IamLightData.Count; i++) {
          var tmp = component.IamLightData[i];
          tmp.Color = color;
          component.IamLightData[i] = tmp;
        }
      }
    }

    public static void ChangePosition(Vector3 position) {
      foreach (var component in Components) {
        for (int i = 0; i < component.IamLightData.Count; i++) {
          var tmp = component.IamLightData[i];
          tmp.Position = position;
          component.IamLightData[i] = tmp;
        }

      }
    }

    private static void SetViewPosition(CLight component) {
      var camera = component.IamEntity.GetComponent<CCamera>();
      var light = component.IamLightData;
      for (int i = 0; i < light.Count; ++i) {
        var buff = light[i];
        buff.ViewPos = camera.Position;
        light[i] = buff;
      }
    }
  }
}
