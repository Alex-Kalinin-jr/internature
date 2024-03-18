

using Assimp.Unmanaged;
using SharpDX.Direct3D;

namespace D3D {
  public class CRenderParams : Component {
    public CPrimitiveTopology IamTopology;


    public CRenderParams(CPrimitiveTopology topology) {
      IamTopology = topology;
    }

    public CRenderParams() {
      IamTopology = new CPrimitiveTopology();
    }

    public CRenderParams(PrimitiveTopology topology) {
      IamTopology = new CPrimitiveTopology(topology);
    }

    public override void UpdateLinks() {
      IamTopology.IamEntity = IamEntity;
    }
  }

}
