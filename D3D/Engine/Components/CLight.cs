using System.Collections.Generic;

namespace D3D {
  public class CLight : Component {
    public List<PsLightConstantBuffer> IamLightData;

    public CLight(List<PsLightConstantBuffer> data) {
      IamLightData = data;
      SLightSystem.Register(this);
    }
  }
}
