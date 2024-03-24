
using SharpDX.Direct3D;

namespace D3D {
  public class CPrimitiveTopology : Component {
    public PrimitiveTopology TopologyObj;

    public CPrimitiveTopology(PrimitiveTopology topology) {
      TopologyObj = topology;
    }

    public CPrimitiveTopology() {
      TopologyObj = PrimitiveTopology.TriangleList;
    }
  }
}
