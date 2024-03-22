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


    public static Scene CreateTestingScene() {
      var scene = new Scene();
      float xVal = 0.0f;
      float zVal = 0.0f;
      for (int i = 0; i < 3; ++i) {
        for (int j = 0; j < 3; ++j) {
          VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
          buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(xVal, 0.0f, zVal));
          scene.AddComponent(new CFigure("Resources/Tree.obj", buff));
          xVal += 5.0f;
        }
        zVal += 5.0f;
        xVal = 0.0f;
      }
      return scene;
    }


    public static Scene CreateGridTestingScene() {
      var scene = new Scene();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
      int x = 10;
      int y = 10;
      int z = 10;
      var fig = new CGridFigure(CreateTestingGridMesh(x, y, z), buff, default, PrimitiveTopology.LineList);
      fig.CurrentXCount = x;
      fig.CurrentYCount = y;
      fig.CurrentZCount = z;
      fig.XCount = x;
      fig.YCount = y;
      fig.ZCount = z;
      scene.AddComponent(fig);
      return scene;
    }


    public static Scene CreateNewGridTestingScene() {
      var scene = new Scene();
      var figures = CreateNewGridFigures(10, 10, 10);
      foreach (var figure in figures) {
        scene.AddComponent(figure);
      }
      return scene;
    }



    public static Scene CreatePipeTestingScene() {
      var scene = new Scene();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
      var mesh = Generator.CreateTestingLineMesh();
      scene.AddComponent(new CFigure(mesh, buff, FigureType.Line, PrimitiveTopology.LineStrip));
      scene.AddComponent(new CFigure(MeshConverter.ConvertToPipe(mesh, 0.5f, 30), buff,
                                     FigureType.Pipe, PrimitiveTopology.LineList));
      return scene;
    }


    public static Scene CreateAnotherPipeTestingScene() {
      var scene = new Scene();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      var mesh = Generator.CreateAnotherTestingLineMesh();
      var pipeMesh = MeshConverter.ConvertToPipe(mesh, 0.5f, 40);
      for (int i = 0; i < 1; ++i) {
        for (int j = 0; j < 1; ++j) {
          // buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5 * i, 0.0f, 5 * j));
          buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 0.0f, 2.0f));
          scene.AddComponent(new CFigure(mesh, buff, FigureType.Line, PrimitiveTopology.LineStrip));
          scene.AddComponent(new CFigure(pipeMesh, buff, FigureType.Pipe, PrimitiveTopology.LineList));
        }

      }
      return scene;
    }


    public static CMesh CreateTestingQuadroMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)), new VsBuffer(new Vector3(1.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(1.0f, 0.0f, 1.0f)), new VsBuffer(new Vector3(0.0f, 0.0f, 1.0f))
      };
      return new CMesh(vertices, new List<short>() { 0, 1, 2, 3, 0 });
    }


    // to be refactored
    public static CMesh CreateTestingGridMesh(int xCount = 30, int yCount = 30, int zCount = 10) {
      List<VsBuffer> vertices = new List<VsBuffer>();
      List<short> indices = new List<short>();
      List<short> pseudoIndices = new List<short>() { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };
      int p = 0;
      var random = new Random();
      float r = (float)random.NextDouble(0.0f, 1.0f);
      float g = (float)random.NextDouble(0.0f, 0.0f);
      float b = (float)random.NextDouble(0.0f, 1.0f);
      for (int i = 0; i < xCount; ++i) {
        Vector3 color = new Vector3(r, g, b);
        for (int j = 0; j < yCount; ++j) {
          for (int k = 0; k < zCount; ++k) {
            vertices.Add(new VsBuffer(new Vector3(i, j, k), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i, j, k + 1), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k + 1), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k + 1), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k + 1), default, default, color));
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k), default, default, color));
            indices.AddRange(pseudoIndices.Select(v => (short)(v + p)));
            p += 8;
          }
        }
      }
      var mesh = new CMesh(vertices, indices);
      return mesh;
    }

    public static List<CNewGridFigure> CreateNewGridFigures(int xCount = 30, int yCount = 30, int zCount = 10) {
      List<CNewGridFigure> output = new List<CNewGridFigure>();
      List<short> pseudoIndices = new List<short>() { 0, 1, 2, 0, 2, 3, 3, 2, 4, 3, 4, 5, 5, 4, 7, 7, 4, 6, 7, 6, 0, 0, 6, 1, 1, 4, 2, 1, 6, 4, 3, 5, 0, 0, 5, 7 };
      var random = new Random();
      for (int i = 0; i < xCount; ++i) {
        float r = (float)random.NextDouble(0.0f, 1.0f);
        float g = (float)random.NextDouble(0.0f, 0.0f);
        float b = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 color = new Vector3(r, g, b);
        for (int j = 0; j < yCount; ++j) {
          for (int k = 0; k < zCount; ++k) {
            List<VsBuffer> vertices = new List<VsBuffer>();
            vertices.Add(new VsBuffer(new Vector3(i, j, k), default, default, color)); //0
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k), default, default, color)); //1
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k), default, default, color)); //2
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k), default, default, color)); //3
            vertices.Add(new VsBuffer(new Vector3(i + 1, j + 1, k + 1), default, default, color)); //4
            vertices.Add(new VsBuffer(new Vector3(i + 1, j, k + 1), default, default, color)); //5
            vertices.Add(new VsBuffer(new Vector3(i, j + 1, k + 1), default, default, color)); //6
            vertices.Add(new VsBuffer(new Vector3(i, j, k + 1), default, default, color)); //7
            var mesh = new CMesh(vertices, pseudoIndices);

            VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
            buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
            var fig = new CNewGridFigure(mesh, buff, FigureType.General, PrimitiveTopology.TriangleList);
            fig.XCoord = i;
            fig.YCoord = j;
            fig.ZCoord = k;
            output.Add(fig);
          }
        }
      }
      return output;
    }


    public static CMesh CreateTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 0.5f, 2.0f)),
          new VsBuffer(new Vector3(0.0f, 0.0f, 4.0f))
      };
      return new CMesh(vertices, new List<short>() { 0, 1, 2 });
    }


    public static CMesh CreateAnotherTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 2.5f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 5.0f, 0.5f)),
          new VsBuffer(new Vector3(0.0f, 8.5f, -0.5f)),
          new VsBuffer(new Vector3(0.0f, 10.5f, 0.5f))
      };
      return new CMesh(vertices, new List<short>() { 0, 1, 2, 3 });
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
