﻿using Assimp;
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
      scene.AddComponent(new CRenderParams(PrimitiveTopology.TriangleList));
      return scene;
    }


    public static Scene CreateGridTestingScene() {
      var scene = new Scene();
      for (int i = 0; i < 10; ++i) {
        for (int j = 0; j < 10; ++j) {
          VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
          buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(i, 0.0f, j));
          scene.AddComponent(new CFigure(CreateTestingQuadroMesh(), buff));
        }
      }
      scene.AddComponent(new CRenderParams(PrimitiveTopology.LineStrip));
      return scene;
    }


    public static Scene CreatePipeTestingScene() {
      var scene = new Scene();
      VsMvpConstantBuffer buff = new VsMvpConstantBuffer();
      buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
      scene.AddComponent(new CFigure(MeshConverter.ConvertToPipe(Generator.CreateTestingLineMesh(), 0.5f, 30), buff));
      scene.AddComponent(new CRenderParams(PrimitiveTopology.LineList));
      return scene;
    }


    public static CMesh CreateTestingQuadroMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)), new VsBuffer(new Vector3(1.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(1.0f, 0.0f, 1.0f)), new VsBuffer(new Vector3(0.0f, 0.0f, 1.0f))
      };
      return new CMesh(vertices, new List<short>() { 0, 1, 2, 3, 0 });
    }

    public static CMesh CreateTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
          new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
          new VsBuffer(new Vector3(0.0f, 0.5f, 2.0f)),
          new VsBuffer(new Vector3(0.0f, 0.0f, 4.0f))
      };
      return new CMesh(vertices, new List<short>() { 0, 1, 2 });
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
