
using SharpDX.Direct3D;

namespace D3D {
  public class CPrimitiveTopology : Component {
    public PrimitiveTopology TopologyObj;

    public CPrimitiveTopology(PrimitiveTopology iamTopology) {
      TopologyObj = iamTopology;
    }

    public CPrimitiveTopology() {
      TopologyObj = PrimitiveTopology.TriangleList;
    }
  }
}
