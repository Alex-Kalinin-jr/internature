using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices; 
    public CTransform TransformObj; 
    public PrimitiveTopology Topology; 


    public CMesh(string path) {
      // Load mesh data from file using a generator method
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);

      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = Generator.ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
      TransformObj = new CTransform(buff);

      Topology = PrimitiveTopology.LineList;
      DrawSystem.Register(this);
    }


    public CMesh() {
      Vertices = new List<VsBuffer>(); 
      Indices = new List<short>();

      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = Generator.ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
      TransformObj = new CTransform(buff);

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
      base.UpdateLinks(); // Call base method to ensure base functionality
      TransformObj.EntityObj = EntityObj;
    }
  }
}