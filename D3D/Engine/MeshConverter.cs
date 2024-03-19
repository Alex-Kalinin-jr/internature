using SharpDX;
using System.Collections.Generic;
using System;
using System.Linq;

namespace D3D {
  public class MeshConverter {

    public static CMesh ConvertToPipe(CMesh lineMesh, float pipeRadius, int segments) {
      var vertices = lineMesh.Vertices.Select(v => v.Position).ToArray();

      if (vertices.Length < 2) {
        return null;
      }

      var pipeVertices = new List<Vector3>(vertices.Length * segments);
      var indices = GenerateIndices(vertices.Length, segments);
      var circleVertices = GenerateCircleVertices(pipeRadius, segments);

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

      List<VsBuffer> points = pipeVertices.Select(v => new VsBuffer(v)).ToList();

      return new CMesh(points, indices);
    }

    private static List<Vector3> GenerateCircleVertices(float pipeRadius, int segments) {
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
            indices.Add((short)(i * numOfSegments + j + 1));
            indices.Add((short)((i + 1) * numOfSegments + j + 1));
            indices.Add((short)((i + 1) * numOfSegments + j + 1));
            indices.Add((short)((i + 1) * numOfSegments + j));
            indices.Add((short)((i + 1) * numOfSegments + j));
          } else {
            indices.Add((short)(i * numOfSegments));
            indices.Add((short)(i * numOfSegments));
            indices.Add((short)((i + 1) * numOfSegments));
            indices.Add((short)((i + 1) * numOfSegments));
            indices.Add((short)((i + 1) * numOfSegments + j));
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
  }
}
