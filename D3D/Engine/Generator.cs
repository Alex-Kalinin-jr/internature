using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
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
    public static Scene CreateGridTestingScene() {
      var scene = new Scene();
      var figure = CreateGridFigures(20, 15, 20); // just an example
      figure.SetProperty(CGridMesh.PropertyType.Color);
      scene.AddComponent(figure);
      return scene;
    }

    /// <summary>
    /// Creates another scene for testing pipe figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreatePipeTestingScene() {
      var scene = new Scene();
      var mesh = CreateTestingLineMesh();
      mesh.TransformObj.TransformObj.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), 
                                                                                new Vector3(0.0f, 0.0f, 0.0f));
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
      var pseudoLineIndices = new List<short>() { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 7, 7, 6, 6, 4, 1, 6, 2, 4, 0, 7, 3, 5 };
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
            if (j < 3) {
              propertyColor.Add(new Vector3(1.0f, 0.0f, 0.0f));
            } else if (j > 2 && j < 5) {
              propertyColor.Add(new Vector3(1.0f, 0.5f, 0.4f));
            } else if (j > 4 && j < 8) {
              propertyColor.Add(new Vector3(0.7f, 0.5f, 0.5f));
            } else if (j > 7 && j < 11) {
              propertyColor.Add(new Vector3(0.5f, 0.8f, 0.9f));
            } else {
              propertyColor.Add(color);
            }
            propertyStability.Add(stability);
            p += 8;
          }
        }
      }
      var mesh = new CGridMesh(vertices, indices, FigureType.Grid);
      mesh.LineIndices = lineIndices;
      mesh.TopologyObj = PrimitiveTopology.TriangleList;
      mesh.AddProperty(CGridMesh.PropertyType.Color, propertyColor.ToArray());
      mesh.AddProperty(CGridMesh.PropertyType.Stability, propertyStability.ToArray());

      var buff = new VsMvpConstantBuffer();
      buff.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

      mesh.Size[0] = xCount;
      mesh.Size[1] = yCount;
      mesh.Size[2] = zCount;

      return mesh;
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

