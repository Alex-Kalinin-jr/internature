using System.Collections.Generic;
using SharpDX.Direct3D;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices;
    public CTransform TransformObj;
    public PrimitiveTopology Topology;

    public CMesh(string path) {
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);
      TransformObj = new CTransform(new VsMvpConstantBuffer());
      Topology = PrimitiveTopology.LineList;
      DrawSystem.Register(this);
    }

    public CMesh() {
      Vertices = new List<VsBuffer>();
      Indices = new List<short>();
      TransformObj = new CTransform(new VsMvpConstantBuffer());
      Topology = PrimitiveTopology.LineList;
      DrawSystem.Register(this);
    }

    public CMesh(List<VsBuffer> vertices, 
                 List<short> indices, 
                 CTransform transform, 
                 PrimitiveTopology topology) {
      Vertices = vertices;
      Indices = indices;
      TransformObj = transform;
      Topology = topology;
      DrawSystem.Register(this);
    }
    public override void UpdateLinks() {
      base.UpdateLinks();
      TransformObj.EntityObj = EntityObj;
    }
  }
}
