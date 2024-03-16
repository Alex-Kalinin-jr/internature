using SharpDX;
using System.Collections.Generic;

namespace D3D {
  public class CWorldPositions : Component {
    public List<Matrix> WorldMatrices;

    public CWorldPositions(List<Matrix> worldMatrices) {
      WorldMatrices = worldMatrices;
      WorldSystem.Register(this);
    }
  }
}
