using SharpDX;
using System.Collections.Generic;
using System;
using System.Linq;

namespace D3D {
  public class MeshConverter {

    public static List<CMesh> GeneratePipe(Vector3[] vertices, float pipeRadius, int segments) {
      if (vertices.Length < 2) {
        return null;
      }

      var pipeVertices = new List<Vector3>();
      var indices = GenerateIndices(vertices.Length, segments);
      var circleVertices = GenerateCircleOfRadAndSegments(pipeRadius, segments);

      Vector3 startPoint = vertices[0];
      Vector3 endPoint = vertices[1];
      Vector3 direction = Vector3.Normalize(endPoint - startPoint);

      for (int i = 0; i < vertices.Length; ++i) {
        startPoint = vertices[i];

        if (i != vertices.Length - 1) {
          endPoint = vertices[i + 1];
          direction = Vector3.Normalize(endPoint - startPoint);
        }

        var buffVertices = circleVertices.Select(v => v + startPoint).ToList();
        var rotatedVertices = RotateVertices(buffVertices, direction);
        pipeVertices.AddRange(rotatedVertices);
      }

      // var meshes = GenerateMeshes();
      return null;
    }

    /*
    private List<CMesh> GenerateMeshes() {
      List<CMesh> meshes = new List<CMesh>();
      for (int i = 0, j = 0; i < vertices.Length; ++i) {
        List<VsBuffer> outVertices = new List<VsBuffer>(4);
        List<short> outIndices = new List<short>(5);
        VsBuffer vsBuffer = new VsBuffer();
        vsBuffer.Position = vertices[i]; 
        outVertices.Add(vsBuffer);
        vsBuffer.Position = vertices[i + 1];
        outVertices.Add(vsBuffer);
        vsBuffer.Position = vertices[i + 2];
        outVertices.Add(vsBuffer);
        vsBuffer.Position = vertices[i + 3];
        outVertices.Add(vsBuffer);

        outIndices.Add(indices[j++]);
        outIndices.Add(indices[j++]);
        outIndices.Add(indices[j++]);
        outIndices.Add(indices[j++]);
        outIndices.Add(indices[j++]);
        meshes.Add(new CMesh(outVertices, outIndices));
        return meshes;
      }
    }
     */

    private static List<Vector3> GenerateCircleOfRadAndSegments(float pipeRadius, int segments) {
      List<Vector3> circleVertices = new List<Vector3>();
      for (int i = 0; i < segments; ++i) {
        float angle = (float)(2 * Math.PI * i / segments);
        float x = (float)(pipeRadius * Math.Cos(angle));
        float y = (float)(pipeRadius * Math.Sin(angle));
        circleVertices.Add(new Vector3(x, y, 0));
      }
      return circleVertices;
    }


    private static List<short> GenerateIndices(int verticesLength, int numOfSegments) {
      List<short> indices = new List<short>();
      for (int i = 0; i < verticesLength - 1; ++i) {
        for (int j = 0; j < numOfSegments; ++j) {
          indices.Add((short)(i * numOfSegments + j));
          if (j != numOfSegments - 1) {
            indices.Add((short)(i * numOfSegments + j + 1));
            indices.Add((short)((i + 1) * numOfSegments + j + 1));
            indices.Add((short)((i + 1) * numOfSegments + j));
          } else {
            indices.Add((short)(i * numOfSegments));
            indices.Add((short)((i + 1) * numOfSegments));
            indices.Add((short)((i + 1) * numOfSegments + j));
          }
          indices.Add((short)(i * numOfSegments + j));
        }
      }
      return indices;
    }


    public static Vector3[] RotateVertices(List<Vector3> vertices, Vector3 direction) {
      Vector3 normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]);
      normal.Normalize();

      Vector3 rotationAxis = Vector3.Cross(normal, direction);
      rotationAxis.Normalize();

      double rotationAngle = Math.Acos(Vector3.Dot(normal, direction));

      Quaternion rotationQuaternion = Quaternion.RotationAxis(rotationAxis, (float)rotationAngle);

      Vector3[] rotatedVertices = new Vector3[vertices.Count];
      for (int i = 0; i < vertices.Count; i++) {
        Vector3 rotatedVertex = Vector3.Transform(vertices[i] - vertices[0], rotationQuaternion);
        rotatedVertices[i] = rotatedVertex + vertices[0];
      }

      return rotatedVertices;
    }

    public static Vector3[] Convert(VsBuffer[] verts) {
      var output = new Vector3[verts.Length];
      for (int i = 0; i < verts.Length; ++i) {
        output[i] = verts[i].Position;
      }
      return output;
    }

  }
}
