using Assimp;
using SharpDX;

namespace D3D {
  public class SLightSystem : BaseSystem<CLight> {
    new public static void Update() {
      foreach (var component in Components) {

        SetViewPosition(component);

        var color = component.IamEntity.GetComponent<CNewLightColor>();
        if (color != null) {
          ChangeColor(component, color.Color);
        }

        var pos = component.IamEntity.GetComponent<CNewLightPosition>();
        if (pos != null) {
          ChangePosition(component, pos.Position);
        }
      }
    }

    private static void ChangeColor(CLight component, Vector4 color) {
      for (int i = 0; i < component.IamLightData.Count; i++) {
        var tmp = component.IamLightData[i];
        tmp.Color = color;
        component.IamLightData[i] = tmp;
      }
      component.IamEntity.RemoveComponent<CNewLightColor>();
    }

    private static void ChangePosition(CLight component, Vector3 position) {
      for (int i = 0; i < component.IamLightData.Count; i++) {
        var tmp = component.IamLightData[i];
        tmp.Position = position;
        component.IamLightData[i] = tmp;
      }
      component.IamEntity.RemoveComponent<CNewLightPosition>();
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
