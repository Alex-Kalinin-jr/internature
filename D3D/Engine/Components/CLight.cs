using System.Collections.Generic;

namespace D3D {
  public class CLight : Component {
    public List<PsLightConstantBuffer> LightDataObj;

    public CLight(List<PsLightConstantBuffer> data) {
      LightDataObj = data;
      LightSystem.Register(this);
    }
  }
}
