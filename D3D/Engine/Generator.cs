using Assimp;
using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D3D {
  // this is just debugging class. Imagine all generations you want for setting your scene, properties and so on.
  public class Generator {

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

    public static Scene CreateNewGridTestingScene() {
      var scene = new Scene();
      scene.AddComponent(CreateNewGridFigures(20, 20, 10));
      return scene;
    }

    public static Scene CreateAnotherPipeTestingScene() {
      var scene = new Scene();
      var mesh = CreateAnotherTestingLineMesh();
      var pipeMesh = MeshConverter.ConvertToPipe(mesh, 0.5f, 40);
      scene.AddComponent(mesh);
      scene.AddComponent(pipeMesh);
      return scene;
    }

    public static CMesh CreateNewGridFigures(int xCount = 30, int yCount = 30, int zCount = 10) {

      List<short> pseudoIndices = new List<short>() { 0, 1, 2, 0, 2, 3, 3, 2, 4, 3, 4, 5, 5, 4, 7, 7, 4, 6, 7, 6, 0, 0, 6, 1, 1, 4, 2, 1, 6, 4, 3, 5, 0, 0, 5, 7 };
      var mesh = new CMesh();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

      var random = new Random();
      for (int i = 0; i < xCount; ++i) {
        float r = (float)random.NextDouble(0.0f, 1.0f);
        float g = (float)random.NextDouble(0.0f, 0.0f);
        float b = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 color = new Vector3(r, g, b);
        for (int j = 0; j < yCount; ++j) {
          for (int k = 0; k < zCount; ++k) {
            List<VsBuffer> vertices = new List<VsBuffer>();
            vertices.Add(new VsBuffer(new Vector3(i, j, k), default, default, color, new int[3] { i, j, k })); //0
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k), default, default, color, new int[3] { i, j, k })); //1
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k), default, default, color, new int[3] { i, j, k })); //2
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k), default, default, color, new int[3] { i, j, k })); //3
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k + 1), default, default, color, new int[3] { i, j, k })); //4
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k + 1), default, default, color, new int[3] { i, j, k })); //5
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k + 1), default, default, color, new int[3] { i, j, k })); //6
            vertices.Add(new VsBuffer(new Vector3(i, j, k + 1), default, default, color, new int[3] { i, j, k })); //7
            mesh.Vertices.AddRange(vertices);
            mesh.Indices.AddRange(pseudoIndices.Select(v => (short)(v + i * 36)));
          }
        }
      }
      return mesh;
    }

    public static CMesh CreateAnotherTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 2.5f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 5.0f, 0.5f)),
          new VsBuffer(new Vector3(0.0f, 8.5f, -0.5f)),
          new VsBuffer(new Vector3(0.0f, 10.5f, 0.5f))
      };

      return new CMesh(vertices,
                      new List<short>() { 0, 1, 2, 3 },
                      new CTransform(new VsMvpConstantBuffer()),
                      PrimitiveTopology.LineStrip);
    }

    public static List<PsLightConstantBuffer> CreateTestingPsLightConstantBuffers() {
      var output = new List<PsLightConstantBuffer> {
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -1.0f)),
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -3.0f)),
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -4.0f))
      };
      return output;
    }

    public static Matrix ComputeTestingModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      buff.Transpose();
      return buff;
    }

  }
}
