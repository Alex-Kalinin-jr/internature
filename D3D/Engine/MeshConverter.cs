using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D3D {
  /// <summary>
  /// Class responsible for converting line meshes into pipe meshes.
  /// </summary>
  public class MeshConverter {
    /// <summary>
    /// Converts a line mesh into a pipe mesh.
    /// </summary>
    /// <param name="lineMesh">The line mesh to convert.</param>
    /// <param name="pipeRadius">The radius of the pipe.</param>
    /// <param name="segments">The number of segments to use for generating the pipe.</param>
    /// <returns>The converted pipe mesh.</returns>
    public static CMesh ConvertToPipe(CMesh lineMesh, float pipeRadius, int segments) {
      // Extract positions of vertices from the line mesh
      var vertices = lineMesh.Vertices.Select(v => v.Position).ToArray();

      // If there are not enough vertices, return null
      if (vertices.Length < 2) {
        return null;
      }

      var pipeVertices = new List<Vector3>(vertices.Length * segments);
      var indices = GenerateIndices(vertices.Length, segments);
      var circleVertices = GenerateCircleVertices(pipeRadius, segments);

      Vector3 startPoint = vertices[0];
      Vector3 endPoint = vertices[1];
      Vector3 direction = Vector3.Normalize(endPoint - startPoint);

      // Reusable buffer for rotated vertices
      var rotatedVertices = new Vector3[circleVertices.Count];

      for (int i = 0; i < vertices.Length; ++i) {
        startPoint = vertices[i];

        if (i != vertices.Length - 1) {
          endPoint = vertices[i + 1];
          direction = Vector3.Normalize(endPoint - startPoint);
        }

        // Rotate circle vertices and add to pipeVertices
        RotateVertices(circleVertices, startPoint, direction, rotatedVertices);
        pipeVertices.AddRange(rotatedVertices);
      }

      // Convert pipeVertices to VsBuffer list
      List<VsBuffer> points = pipeVertices.Select(v => new VsBuffer(v)).ToList();

      var mesh = new CMesh(points, indices, FigureType.Pipe);
      mesh.TopologyObj = SharpDX.Direct3D.PrimitiveTopology.LineList;
      mesh.TransformObj = lineMesh.TransformObj;

      return mesh;
    }

    /// <summary>
    /// Generates circle vertices for creating the pipe.
    /// </summary>
    /// <param name="pipeRadius">The radius of the pipe.</param>
    /// <param name="segments">The number of segments to use for generating the pipe.</param>
    /// <returns>The list of generated circle vertices.</returns>
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

    /// <summary>
    /// Generates indices for creating the pipe.
    /// </summary>
    /// <param name="verticesLength">The length of the vertices array.</param>
    /// <param name="numOfSegments">The number of segments to use for generating the pipe.</param>
    /// <returns>The list of generated indices.</returns>
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

    /// <summary>
    /// Rotates vertices based on the given direction.
    /// </summary>
    /// <param name="vertices">The list of vertices to rotate.</param>
    /// <param name="startPoint">The start point for rotation.</param>
    /// <param name="direction">The direction vector.</param>
    /// <param name="result">The array to store the rotated vertices.</param>
    private static void RotateVertices(List<Vector3> vertices, Vector3 startPoint, Vector3 direction, Vector3[] result) {
      Vector3 normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]);
      normal.Normalize();

      Vector3 rotationAxis = Vector3.Cross(normal, direction);
      rotationAxis.Normalize();

      double rotationAngle = Math.Acos(Vector3.Dot(normal, direction));

      Quaternion rotationQuaternion = Quaternion.RotationAxis(rotationAxis, (float)rotationAngle);

      for (int i = 0; i < vertices.Count; ++i) {
        Vector3 rotatedVertex = Vector3.Transform(vertices[i] - vertices[0], rotationQuaternion);
        result[i] = rotatedVertex + startPoint;
      }
    }
  }
}
