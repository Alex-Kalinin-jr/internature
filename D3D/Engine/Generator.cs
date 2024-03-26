using Assimp;
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
    /// Generates mesh data from a file.
    /// </summary>
    /// <param name="FileName">The file name of the mesh.</param>
    /// <returns>A tuple containing lists of vertices and indices.</returns>
    public static (List<VsBuffer>, List<short>) GenerateMeshFromFile(string FileName) {
      var vertices = new List<VsBuffer>();
      var indices = new List<short>();
      PostProcessSteps Flags = PostProcessSteps.GenerateSmoothNormals
                             | PostProcessSteps.CalculateTangentSpace
                             | PostProcessSteps.Triangulate;

      AssimpContext importer = new AssimpContext();
      Assimp.Scene model = importer.ImportFile(FileName, Flags);

      foreach (Mesh mesh in model.Meshes) {
        for (int i = 0; i < mesh.VertexCount; ++i) {
          Vector3D Pos = mesh.Vertices[i];
          Vector3D Normal = mesh.Normals[i];
          Vector3D Tex = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i] : new Vector3D();
          vertices.Add(new VsBuffer(new Vector3(Pos.X, Pos.Y, Pos.Z),
                                     new Vector3(Normal.X, Normal.Y, Normal.Z),
                                     new Vector2(Tex.X, Tex.Y)));
        }

        int indexBase = (short)indices.Count();
        foreach (Face Faces in mesh.Faces) {
          if (Faces.IndexCount != 3)
            continue;
          indices.Add((short)(indexBase + Faces.Indices[0]));
          indices.Add((short)(indexBase + Faces.Indices[1]));
          indices.Add((short)(indexBase + Faces.Indices[2]));
        }
      }
      return (vertices, indices);
    }

    /// <summary>
    /// Creates a new scene for testing grid figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreateNewGridTestingScene() {
      var scene = new Scene();
      var figure = CreateNewGridFigures(20, 20, 10);
      figure.SetProperty(CGridMesh.PropertyType.Stability);
      scene.AddComponent(figure);
      return scene;
    }

    /// <summary>
    /// Creates another scene for testing pipe figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreateAnotherPipeTestingScene() {
      var scene = new Scene();
      var mesh = CreateAnotherTestingLineMesh();
      var pipeMesh = MeshConverter.ConvertToPipe(mesh, 0.5f, 40);
      scene.AddComponent(mesh);
      scene.AddComponent(pipeMesh);
      return scene;
    }

    /// <summary>
    /// Creates grid figures.
    /// </summary>
    /// <param name="xCount">The number of grid cells along the x-axis.</param>
    /// <param name="yCount">The number of grid cells along the y-axis.</param>
    /// <param name="zCount">The number of grid cells along the z-axis.</param>
    /// <returns>The created grid mesh.</returns>
    public static CGridMesh CreateNewGridFigures(int xCount = 30, int yCount = 30, int zCount = 10) {
      List<VsBuffer> vertices = new List<VsBuffer>();
      var indices = new List<short>();
      var pseudoIndices = new List<short>() { 0, 1, 2, 0, 2, 3, 3, 2, 4, 3, 4, 5,
                                                            5, 4, 7, 7, 4, 6, 7, 6, 0, 0, 6, 1,
                                                            1, 4, 2, 1, 6, 4, 3, 5, 0, 0, 5, 7 };
      var lineIndices = new List<short>();
      var pseudoLineIndices = new List<short>() { 0, 1, 1, 2, 2, 3, 3, 0, 2, 4, 4, 5, 5, 3, 4, 6, 6, 7, 7, 5, 0, 7, 1, 6 };
      var propertyColor = new List<Vector3>();
      var propertyStability = new List<Vector3>();
      var random = new Random();
      int p = 0;


      for (int j = 0; j < yCount; ++j) {

        float r = (float)random.NextDouble(0.0f, 1.0f);
        float g = (float)random.NextDouble(0.0f, 0.0f);
        float b = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 color = new Vector3(r, g, b);

        for (int i = 0; i < xCount; ++i) {
          for (int k = 0; k < zCount; ++k) {
            float a = (float)random.NextDouble(0.0f, 1.0f);
            float aa = (float)random.NextDouble(0.0f, 0.0f);
            float aaa = (float)random.NextDouble(0.0f, 1.0f);
            Vector3 stability = new Vector3(a, aa, aaa);

            var pseudoVertices = new List<VsBuffer>();
            vertices.Add(new VsBuffer(new Vector3(i, j, k), default, default, color, i, j, k)); //0
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k), default, default, color, i, j, k)); //1
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k), default, default, color, i, j, k)); //2
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k), default, default, color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k + 1), default, default, color, i, j, k)); //4
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k + 1), default, default, color, i, j, k)); //5
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k + 1), default, default, color, i, j, k)); //6
            vertices.Add(new VsBuffer(new Vector3(i, j, k + 1), default, default, color, i, j, k)); //7
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
      mesh.AddProperty(CGridMesh.PropertyType.Color, propertyColor.ToArray());
      mesh.AddProperty(CGridMesh.PropertyType.Stability, propertyStability.ToArray());

      var buff = new VsMvpConstantBuffer();
      buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

      return mesh;
    }

    /// <summary>
    /// Creates a line mesh for testing.
    /// </summary>
    /// <returns>The created line mesh.</returns>
    public static CMesh CreateAnotherTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
                new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 2.5f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 5.0f, 0.5f)),
                new VsBuffer(new Vector3(0.0f, 8.5f, -0.5f)),
                new VsBuffer(new Vector3(0.0f, 10.5f, 0.5f))
            };
      return new CMesh(vertices, new List<short>() { 0, 1, 2, 3 }, FigureType.Line);
    }

    /// <summary>
    /// Creates testing pixel shader light constant buffers.
    /// </summary>
    /// <returns>The list of created pixel shader light constant buffers.</returns>
    public static List<PsLightConstantBuffer> CreateTestingPsLightConstantBuffers() {
      var output = new List<PsLightConstantBuffer> {
                new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -1.0f)),
                new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -3.0f)),
                new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -4.0f))
            };
      return output;
    }

    /// <summary>
    /// Computes the testing model matrix.
    /// </summary>
    /// <param name="rotations">The rotation vector.</param>
    /// <param name="translations">The translation vector.</param>
    /// <returns>The computed model matrix.</returns>
    public static Matrix ComputeTestingModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      buff.Transpose();
      return buff;
    }

  }
}

