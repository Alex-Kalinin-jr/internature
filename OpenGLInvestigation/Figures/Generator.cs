using System;
using System.Numerics;

namespace OpenGLInvestigation.Figures {
  internal class Generator {

    public static (float[], uint[], float[], float[]) GenerateCube(int count, OpenTK.Mathematics.Vector3 color) {
      
      int numVertices = count * count * count;
      int numIndices = count * count * count * 6;

      float[] cube = new float[numVertices * 3];
      uint[] idxs = new uint[numIndices];
      float step = 2.0f / (count - 1);
      GenerateVerticesAndIndices(count, step, ref cube, ref idxs);

      float[] colors = new float[cube.Length];
      GenerateColorArray(ref colors, color);

      float[] normals = new float[cube.Length];
      GenerateNormals(ref cube, ref idxs, ref normals);

      return (cube, idxs, colors, normals);
    }


    private static void GenerateNormals(ref float[] vertices, ref uint[] indices, ref float[] normals) {

      for (int i = 0; i < indices.Length; i += 3) {
        uint index1 = indices[i];
        uint index2 = indices[i + 1];
        uint index3 = indices[i + 2];

        Vector3 v1 = new Vector3(vertices[index1 * 3], vertices[index1 * 3 + 1], vertices[index1 * 3 + 2]);
        Vector3 v2 = new Vector3(vertices[index2 * 3], vertices[index2 * 3 + 1], vertices[index2 * 3 + 2]);
        Vector3 v3 = new Vector3(vertices[index3 * 3], vertices[index3 * 3 + 1], vertices[index3 * 3 + 2]);
        Vector3 edge1 = v2 - v1;
        Vector3 edge2 = v3 - v1;
        Vector3 triangleNormal = Vector3.Cross(edge1, edge2);

        normals[index1 * 3] += triangleNormal.X;
        normals[index1 * 3 + 1] += triangleNormal.Y;
        normals[index1 * 3 + 2] += triangleNormal.Z;
        normals[index2 * 3] += triangleNormal.X;
        normals[index2 * 3 + 1] += triangleNormal.Y;
        normals[index2 * 3 + 2] += triangleNormal.Z;
        normals[index3 * 3] += triangleNormal.X;
        normals[index3 * 3 + 1] += triangleNormal.Y;
        normals[index3 * 3 + 2] += triangleNormal.Z;
      }

      for (int i = 0; i < normals.Length; i += 3) {
        Vector3 normal = new Vector3(normals[i], normals[i + 1], normals[i + 2]);
        normal = Vector3.Normalize(normal);
        normals[i] = normal.X;
        normals[i + 1] = normal.Y;
        normals[i + 2] = normal.Z;
      }
    }


    private static void GenerateVerticesAndIndices(int count, float step, ref float[] vertices, ref uint[] indices) {

      int v = 0;
      int i = 0;

      for (int x = 0; x < count; x++) {
        for (int y = 0; y < count; y++) {
          for (int z = 0; z < count; z++) {
            vertices[v] = x * step;
            vertices[v + 1] = y * step;
            vertices[v + 2] = z * step;
            v += 3;

            uint start = (uint)(x * count * count + y * count + z);

            indices[i] = start;
            indices[i + 1] = start + 1;
            indices[i + 2] = start + (uint)count;
            indices[i + 3] = start + 1;
            indices[i + 4] = start + (uint)count + 1;
            indices[i + 5] = start + (uint)count;
            i += 6;
          }
        }
      }
    }

    private static void GenerateColorArray(ref float[] colors, OpenTK.Mathematics.Vector3 color) {
      int idx = 0;
      while (idx < colors.Length) {
        colors[idx] = color.X;
        colors[idx + 1] = color.Y;
        colors[idx + 2] = color.Z;
        idx += 3;
      }

    }

    public static (float[], float[], float[]) GenerateTestingCube() {
      float[] vertices = {
            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
        };

      float[] normals = {
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f, -1.0f,
          0.0f,  0.0f,  1.0f,
          0.0f,  0.0f,  1.0f,
          0.0f,  0.0f,  1.0f,
          0.0f,  0.0f,  1.0f,
          0.0f,  0.0f,  1.0f,
          0.0f,  0.0f,  1.0f,
         -1.0f,  0.0f,  0.0f,
         -1.0f,  0.0f,  0.0f,
         -1.0f,  0.0f,  0.0f,
         -1.0f,  0.0f,  0.0f,
         -1.0f,  0.0f,  0.0f,
         -1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          1.0f,  0.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f, -1.0f,  0.0f,
          0.0f,  1.0f,  0.0f,
          0.0f,  1.0f,  0.0f,
          0.0f,  1.0f,  0.0f,
          0.0f,  1.0f,  0.0f,
          0.0f,  1.0f,  0.0f,
          0.0f,  1.0f,  0.0f
      };

      float[] colors = {
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,

        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,

        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,

        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,

        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,

        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f,
        1.0f, 0.5f, 0.0f
      };

      return (vertices, colors, normals);
    }
  }
}
