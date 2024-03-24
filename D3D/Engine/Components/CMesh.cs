using System.Collections.Generic;
using SharpDX.Direct3D;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices; 
    public CTransform TransformObj; 
    public PrimitiveTopology Topology; 

    // Constructor to initialize CMesh with mesh data loaded from a file
    public CMesh(string path) {
      // Load mesh data from file using a generator method
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);
      TransformObj = new CTransform(new VsMvpConstantBuffer()); // Create a new transform object
      Topology = PrimitiveTopology.LineList;
      DrawSystem.Register(this); // Register this mesh with the draw system for rendering
    }

    // Default constructor to initialize an empty mesh
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
      base.UpdateLinks(); // Call base method to ensure base functionality
      TransformObj.EntityObj = EntityObj;
    }
  }
}