
using SharpDX;

namespace D3D {
  public class CNewLightPosition : Component {
    public Vector3 Position;

    public CNewLightPosition(Vector3 pos) {
      Position = pos;
    }
  }
}
