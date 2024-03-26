using System.Collections.Generic;

namespace D3D {
  /// <summary>
  /// Represents a collection of light data associated with an entity.
  /// </summary>
  public class CLight : Component {
    /// <summary>
    /// List of light data objects.
    /// </summary>
    public List<PsLightConstantBuffer> LightDataObj;

    /// <summary>
    /// Initializes a new instance of the <see cref="CLight"/> class with the specified list of light data.
    /// </summary>
    /// <param name="data">The list of light data.</param>
    public CLight(List<PsLightConstantBuffer> data) {
      LightDataObj = data;
      LightSystem.Register(this);
    }
  }
}
