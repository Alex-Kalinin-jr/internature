using Assimp;
using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D3D {
  // this is just debugging class. Imagine all generations you want for setting your scene, properties and so on.
  public class Generator {

    public static (List<VsBuffer>, List<short>) GenerateMesh(string FileName) {

      var vertices = new List<VsBuffer>();

      var indices = new List<short>();


      PostProcessSteps Flags = PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.Triangulate;

      AssimpContext importer = new AssimpContext();

      Assimp.Scene model = importer.ImportFile(FileName, Flags);


      foreach (Assimp.Mesh mesh in model.Meshes) {

        for (int i = 0; i < mesh.VertexCount; ++i) {
          Vector3D Pos = mesh.Vertices[i];
          Vector3D Normal = mesh.Normals[i];
          Vector3D Tex = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i] : new Vector3D();

          vertices.Add(new VsBuffer(new Vector3(Pos.X, Pos.Y, Pos.Z), new Vector3(Normal.X, Normal.Y, Normal.Z), new Vector2(Tex.X, Tex.Y)));
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
          buff.world = ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
                                                 new Vector3(xVal, 0.0f, zVal));
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


    public static CMesh CreateTestingQuadroMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>();
      List<short> indices = new List<short>();

      var vertex1 = new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f));
      var vertex2 = new VsBuffer(new Vector3(1.0f, 0.0f, 0.0f));
      var vertex3 = new VsBuffer(new Vector3(1.0f, 0.0f, 1.0f));
      var vertex4 = new VsBuffer(new Vector3(0.0f, 0.0f, 1.0f));

      vertices.Add(vertex1);
      vertices.Add(vertex2);
      vertices.Add(vertex3);
      vertices.Add(vertex4);

      indices.Add(0);
      indices.Add(1);
      indices.Add(2);
      indices.Add(3);

      return new CMesh(vertices, indices);
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


    public static (Vector3[] vertices, short[] indices) GeneratePipe(Vector3[] vertices) {
      if (vertices.Length < 2) {
        return (null, null);
      }

      Vector3[] pathVertices = vertices;
      float pipeRadius = 0.5f; // Example radius 
      int segments = 4; // Number of segments for the circle
      List<Vector3> circleVertices = new List<Vector3>();

      for (int i = 0; i < segments; ++i) {
        float angle = (float)(2 * Math.PI * i / segments);
        float x = (float)(pipeRadius * Math.Cos(angle));
        float y = (float)(pipeRadius * Math.Sin(angle));
        circleVertices.Add(new Vector3(x, y, 0));
      }

      List<Vector3> pipeVertices = new List<Vector3>();
      Vector3 startPoint = pathVertices[0];
      Vector3 endPoint = pathVertices[1];
      Vector3 direction = Vector3.Normalize(endPoint - startPoint);

      for (int i = 0; i < pathVertices.Length; ++i) {
        startPoint = pathVertices[i];

        if (i != pathVertices.Length - 1) {
          endPoint = pathVertices[i + 1];
          direction = Vector3.Normalize(endPoint - startPoint);
        }

        var buffVertices = new List<Vector3>();
        buffVertices.AddRange(circleVertices);

        for (int j = 0; j < buffVertices.Count; ++j) {
          buffVertices[j] += startPoint;
        }

        var rotatedVertices = RotateVertices(buffVertices, direction);
        pipeVertices.AddRange(rotatedVertices);
      }

      List<short> indices = new List<short>();
      for (int i = 0; i < vertices.Length - 1; ++i) {
        for (int j = 0; j < segments; ++j) {
          indices.Add((short)(i * segments + j));
          if (j != segments - 1) {
            indices.Add((short)(i * segments + j + 1));
            indices.Add((short)((i + 1) * segments + j + 1));
            indices.Add((short)((i + 1) * segments + j));
          } else {
            indices.Add((short)(i * segments));
            indices.Add((short)((i + 1) * segments));
            indices.Add((short)((i + 1) * segments + j));
          }
        }
      }

      return (pipeVertices.ToArray(), indices.ToArray());
    }

/*
public static Vector3[] RotateVertices(List<Vector3> vertices, Vector3 direction) {
    Vector3 normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]);
    normal.Normalize();

    Vector3 rotationAxis = Vector3.Cross(normal, direction);
    rotationAxis.Normalize();

    float rotationAngle = Vector3.Dot(normal, direction);

    Matrix rotationMatrix = Matrix.RotationAxis(rotationAxis, rotationAngle);

    Vector3[] rotatedVertices = new Vector3[vertices.Count];
    for (int i = 0; i < vertices.Count; i++) {
      Vector4 rotatedVertex = Vector4.Transform(new Vector4(vertices[i] - vertices[0], 1.0f), rotationMatrix) + new Vector4(vertices[0], 1.0f);
      rotatedVertices[i] = new Vector3(rotatedVertex.X, rotatedVertex.Y, rotatedVertex.Z);
    }

    return rotatedVertices;
  }
 */

public static Vector3[] RotateVertices(List<Vector3> vertices, Vector3 direction) {
    Vector3 normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]);
    normal.Normalize();

    Vector3 rotationAxis = Vector3.Cross(normal, direction);
    rotationAxis.Normalize();

    double rotationAngle = Math.Acos(Vector3.Dot(normal, direction));

    SharpDX.Quaternion rotationQuaternion = SharpDX.Quaternion.RotationAxis(rotationAxis, (float)rotationAngle);

    Vector3[] rotatedVertices = new Vector3[vertices.Count];
    for (int i = 0; i < vertices.Count; i++) {
      Vector3 rotatedVertex = Vector3.Transform(vertices[i] - vertices[0], rotationQuaternion);
      rotatedVertices[i] = rotatedVertex + vertices[0];
    }

    return rotatedVertices;
  }




  // function to convert VSbuffer positions to array of vector
  public static Vector3[] Convert(VsBuffer[] verts) {
      var output = new Vector3[verts.Length];
      for (int i = 0; i < verts.Length; ++i) {
        output[i] = verts[i].Position;
      }
      return output;
    }

    public static VsBuffer[] GenerateTestPipe() {
      VsBuffer[] buff = new VsBuffer[3];
      buff[0].Position = new Vector3(0.0f, 0.0f, 0.0f);
      buff[1].Position = new Vector3(0.0f, 1.0f, 0.0f);
      buff[2].Position = new Vector3(0.0f, 2.0f, 0.0f);
      return buff;
    }
  }
}
