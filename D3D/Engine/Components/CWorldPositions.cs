using SharpDX;
using System.Collections.Generic;

namespace D3D {
  public class CWorldPositions : Component {
    public List<Matrix> IamWorldMatrices;

    public CWorldPositions(List<Matrix> worldMatrices) {
      IamWorldMatrices = worldMatrices;
      WorldSystem.Register(this);
    }
  }
}
