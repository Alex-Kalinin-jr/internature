using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  /// <summary>
  /// Represents a mesh component with vertices and indices.
  /// </summary>
  public class CMesh : Component {
    /// <summary>
    /// List of vertices.
    /// </summary>
    public List<VsBuffer> Vertices;

    /// <summary>
    /// List of indices.
    /// </summary>
    public List<short> Indices;

    /// <summary>
    /// Transform component associated with the mesh.
    /// </summary>
    public CTransform TransformObj;

    /// <summary>
    /// Primitive topology of the mesh.
    /// </summary>
    public PrimitiveTopology TopologyObj;

    /// <summary>
    /// Type of the figure represented by the mesh.
    /// </summary>
    public FigureType FigureTypeObj;

    /// <summary>
    /// Initializes a new instance of the <see cref="CMesh"/> class with mesh data loaded from a file.
    /// </summary>
    /// <param name="path">The path to the mesh file.</param>
    public CMesh(string path) {
      Initialize();
      (Vertices, Indices) = Generator.GenerateMeshFromFile(path);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CMesh"/> class.
    /// </summary>
    public CMesh() {
      Initialize();
      Vertices = new List<VsBuffer>();
      Indices = new List<short>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CMesh"/> class with the specified vertices, indices, and figure type.
    /// </summary>
    /// <param name="vertices">The list of vertices.</param>
    /// <param name="indices">The list of indices.</param>
    /// <param name="figureType">The type of the figure.</param>
    public CMesh(List<VsBuffer> vertices, List<short> indices, FigureType figureType = FigureType.General) {
      Initialize();
      Vertices = vertices;
      Indices = indices;
      FigureTypeObj = figureType;
    }

    /// <summary>
    /// Initializes the mesh component with default values.
    /// </summary>
    private void Initialize() {
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 0.0f, 0));
      TransformObj = new CTransform(buff);
      TopologyObj = PrimitiveTopology.LineStrip;
      FigureTypeObj = FigureType.General;
      DrawSystem.Register(this);
    }

    /// <summary>
    /// Updates the links associated with the mesh component.
    /// </summary>
    public override void UpdateLinks() {
      TransformObj.EntityObj = EntityObj;
    }
  }
}
