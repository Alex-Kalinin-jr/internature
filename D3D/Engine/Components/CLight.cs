using System.Collections.Generic;

namespace D3D {
  // Definition of the CLight component, representing a collection of light data
  public class CLight : Component {
    // Field to hold a list of light data objects
    public List<PsLightConstantBuffer> LightDataObj;

    // Constructor to initialize CLight with a list of light data and register it with the light system
    public CLight(List<PsLightConstantBuffer> data) {
      LightDataObj = data;
      LightSystem.Register(this);
    }
  }
}