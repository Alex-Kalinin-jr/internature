using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CMesh : Component {
    public List<VsBuffer> Vertices;
    public List<short> Indices;
    public CTransform TransformObj;
    public PrimitiveTopology TopologyObj;
    public FigureType FigureTypeObj;

    public CMesh(string path) {
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = Generator.ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 0.0f, 0));
      TransformObj = new CTransform(buff);
      TopologyObj = PrimitiveTopology.LineStrip;
      FigureTypeObj = FigureType.General;
      DrawSystem.Register(this);
    }

    public CMesh() {
      Vertices = new List<VsBuffer>();
      Indices = new List<short>();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = Generator.ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 0.0f, 0));
      TransformObj = new CTransform(buff);
      TopologyObj = PrimitiveTopology.LineStrip;
      FigureTypeObj = FigureType.General;
      DrawSystem.Register(this);
    }

    public CMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.General) {
      Vertices = vertices;
      Indices = indices;
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = Generator.ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 0.0f, 0));
      TransformObj = new CTransform(buff);
      TopologyObj = PrimitiveTopology.LineStrip;
      FigureTypeObj = figureType;
      DrawSystem.Register(this);
    }

    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }

  }
}
