using System.Collections.Generic;

namespace D3D {
  public class CLight : Component {
    public List<PsLightConstantBuffer> IamLightData;

    public CLight(List<PsLightConstantBuffer> data) {
      IamLightData = data;
      LightSystem.Register(this);
    }

    public override void Update() {

      var color = IamEntity.GetComponent<CNewLightColor>();
      if (color != null) {
        for (int i = 0; i < IamLightData.Count; i++) {
          var tmp = IamLightData[i];
          tmp.Color = color.Color;
          IamLightData[i] = tmp;
        }
        IamEntity.RemoveComponent<CNewLightColor>();
      }

      var pos = IamEntity.GetComponent<CNewLightPosition>();
      if (pos != null) {
        for (int i = 0; i < IamLightData.Count; i++) {
          var tmp = IamLightData[i];
          tmp.Position = pos.Position;
          IamLightData[i] = tmp;
        }
        IamEntity.RemoveComponent<CNewLightPosition>();
      }
    }
  }
}
