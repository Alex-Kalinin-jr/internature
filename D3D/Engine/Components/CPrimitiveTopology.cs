
using SharpDX.Direct3D;

namespace D3D {
  public class CPrimitiveTopology : Component {
    public PrimitiveTopology IamTopology;

    public CPrimitiveTopology(PrimitiveTopology iamTopology) {
      IamTopology = iamTopology;
    }

    public CPrimitiveTopology() {
      IamTopology = PrimitiveTopology.TriangleList;
    }
  }
}
