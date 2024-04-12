using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace D3D {
  /// <summary>
  /// Debugging class for generating scenes, meshes, and properties.
  /// </summary>
  public class Generator {

    /// <summary>
    /// Creates a new scene for testing grid figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreateGridTestingScene(int[] gridSize) {
      var scene = new Scene();
      var figure = CreateGridFigures(gridSize[0], gridSize[1], gridSize[2]); // just an example
      scene.AddComponent(figure);
      return scene;
    }

    /// <summary>
    /// Creates another scene for testing pipe figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreatePipeTestingScene(float worldX, float worldY, float worldZ) {
      var scene = new Scene();
      var mesh = CreateTestingLineMesh();
      mesh.TransformObj.TransformObj.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
                                                                                new Vector3(worldX, worldY, worldZ));
      MeshConverter.CreatePipe(mesh, 0.3f, 15);
      scene.AddComponent(mesh);
      return scene;
    }

    /// <summary>
    /// Creates grid figures.
    /// </summary>
    /// <param name="xCount">The number of grid cells along the x-axis.</param>
    /// <param name="yCount">The number of grid cells along the y-axis.</param>
    /// <param name="zCount">The number of grid cells along the z-axis.</param>
    /// <returns>The created grid mesh.</returns>
    public static CGridMesh CreateGridFigures(int xCount = 30, int yCount = 30, int zCount = 10) {
      List<VsBuffer> vertices = new List<VsBuffer>();
      var indices = new List<short>();
      var pseudoIndices = new List<short>() { 0, 1, 2, 0, 2, 3, 3, 2, 4, 3, 4, 5,
                                                            5, 4, 7, 7, 4, 6, 7, 6, 0, 0, 6, 1,
                                                            1, 4, 2, 1, 6, 4, 3, 5, 0, 0, 5, 7 };
      var lineIndices = new List<short>();
      var pseudoLineIndices = new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 0, 2, 1, 3, 6, 4, 7, 5, 0, 6, 2, 4, 1, 7, 3, 5 };
      var propertyColor = new List<Vector3>();
      var propertyStability = new List<Vector3>();
      var random = new Random();
      int p = 0;


      for (int j = 0; j < yCount; ++j) {

        float r = (float)random.NextDouble(0.0f, 1.0f);
        float g = (float)random.NextDouble(0.0f, 1.0f);
        float b = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 color = new Vector3(r, g, b);
        float property_2_r = (float)random.NextDouble(0.0f, 1.0f);
        float property_2_g = (float)random.NextDouble(0.0f, 1.0f);
        float property_2_b = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 stability = new Vector3(property_2_r, property_2_g, property_2_b);
        for (int i = 0; i < xCount; ++i) {
          for (int k = 0; k < zCount; ++k) {
            var pseudoVertices = new List<VsBuffer>();
            vertices.Add(new VsBuffer(new Vector3(i, j, k), color, i, j, k)); //0
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k), color, i, j, k)); //1
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k), color, i, j, k)); //2
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k), color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k + 1), color, i, j, k)); //4
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k + 1), color, i, j, k)); //5
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k + 1), color, i, j, k)); //6
            vertices.Add(new VsBuffer(new Vector3(i, j, k + 1), color, i, j, k)); //7
            vertices.AddRange(pseudoVertices);

            indices.AddRange(pseudoIndices.Select(v => (short)(v + p)));
            lineIndices.AddRange(pseudoLineIndices.Select(v => (short)(p + v)));
            propertyColor.Add(color);
            propertyStability.Add(stability);
            p += 8;
          }
        }
      }
      var mesh = new CGridMesh(vertices, indices, FigureType.Grid);
      mesh.LineIndices = lineIndices;
      mesh.TopologyObj = PrimitiveTopology.TriangleList;
      mesh.AddProperty("color", propertyColor.ToArray());
      mesh.AddProperty("stability", propertyStability.ToArray());

      var buff = new VsMvpConstantBuffer();
      buff.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
                                                      new Vector3(0.0f, 0.0f, 0.0f));

      mesh.Size[0] = xCount;
      mesh.Size[1] = yCount;
      mesh.Size[2] = zCount;

      return mesh;
    }

    public static CGridMesh GenerateFromBinary(string filePath) {
      using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) {
        int xCount = reader.ReadInt32();
        int yCount = reader.ReadInt32();
        int zCount = reader.ReadInt32();


        List<VsBuffer> vertices = new List<VsBuffer>();
        var indices = new List<short>();
        var pseudoIndices = new List<short>() { 0, 1, 2, 1, 3, 2, 2, 3, 5, 2, 5, 4, 4, 5, 7, 4, 7, 6, 6, 7, 1, 6, 1, 0, 1, 7, 5, 1, 5, 3, 0, 2, 6, 2, 4, 6 };
        var lineIndices = new List<short>();
        var pseudoLineIndices = new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 0, 2, 1, 3, 6, 4, 7, 5 };
        var propertyColor = new List<Vector3>();
        var propertyStability = new List<Vector3>();
        var random = new Random();
        int p = 0;
        float fact = 0.01f;
        for (int k = 0; k < zCount; ++k) {
          float r = (float)random.NextDouble(0.0f, 1.0f);
          float g = (float)random.NextDouble(0.0f, 1.0f);
          float b = (float)random.NextDouble(0.0f, 1.0f);
          Vector3 color = new Vector3(r, g, b);
          float property_2_r = (float)random.NextDouble(0.0f, 1.0f);
          float property_2_g = (float)random.NextDouble(0.0f, 1.0f);
          float property_2_b = (float)random.NextDouble(0.0f, 1.0f);
          Vector3 stability = new Vector3(property_2_r, property_2_g, property_2_b);
          for (int i = 0; i < xCount; ++i) {
            for (int j = 0; j < yCount; ++j) {
              reader.ReadBoolean();
              var pseudoVertices = new List<VsBuffer>();
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //0
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //1
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //2
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //3
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //3
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //3
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //3
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * fact, reader.ReadSingle() * fact, reader.ReadSingle() * fact), color, i, j, k)); //3

              indices.AddRange(pseudoIndices.Select(v => (short)(v + p)));
              lineIndices.AddRange(pseudoLineIndices.Select(v => (short)(p + v)));
              propertyColor.Add(color);
              propertyStability.Add(stability);
              p += 8;

            }
          }
        }
        var mesh = new CGridMesh(vertices, indices, FigureType.Grid);
        mesh.LineIndices = lineIndices;
        mesh.TopologyObj = PrimitiveTopology.TriangleList;
        mesh.AddProperty("color", propertyColor.ToArray());
        mesh.AddProperty("stability", propertyStability.ToArray());

        var buff = new VsMvpConstantBuffer();
        buff.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

        mesh.Size[0] = xCount;
        mesh.Size[1] = yCount;
        mesh.Size[2] = zCount;

        return mesh;
      }
    }


    public static Scene CreateGridScene(CGridMesh grid) {
      var scene = new Scene();
      scene.AddComponent(grid);
      return scene;
    }


    /// <summary>
    /// Creates a line mesh for testing.
    /// </summary>
    /// <returns>The created line mesh.</returns>
    public static CMesh CreateTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
                new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 4.5f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 10.0f, 0.5f)),
                new VsBuffer(new Vector3(0.0f, 16.5f, -0.5f)),
                new VsBuffer(new Vector3(0.0f, 20.5f, 0.5f))
            };
      var mesh = new CMesh(vertices, new List<short>() { 0, 1, 2, 3, 4 }, FigureType.Line);
      mesh.TransformObj.TransformObj.world = TransformSystem.ComputeModelMatrix(new Vector3(0, 0, 0), new Vector3(5, 0, 5));
      return mesh;
    }

  }
}

