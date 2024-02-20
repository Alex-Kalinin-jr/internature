using System;
using System.Numerics;

namespace OpenGLInvestigation.Figures {
  internal class Generator {

    public static (float[], uint[], float[], float[]) GenerateCube(int count, OpenTK.Mathematics.Vector3 color) {

      float[] cube = new float[3 * count * count * 6];
      uint[] idxs = new uint[(count - 1) * (count - 1) * 2 * 6 * 3];
      float[] colors = new float[cube.Length];
      float[] normals = new float[cube.Length];

      float step = 2.0f / (count - 1);
      int ind = 0;

      GenerateFrozenX(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenX(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenZ(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenZ(count, step, ref cube, ref ind, 1.0f);

      GenerateIndexArray(count, ref idxs);
      GenerateColorArray(ref colors, color);
      GenerateNormals(ref cube, ref idxs, ref normals);

      return (cube, idxs, colors, normals);
    }

    private static void GenerateNormals(ref float[] cube, ref uint[] indices, ref float[] normals) {
      Vector3[] normalsVrs = new Vector3[cube.Length / 3];

      for (int i = 0; i < indices.Length; i += 3) {
        uint index1 = indices[i];
        uint index2 = indices[i + 1];
        uint index3 = indices[i + 2];

        Vector3 v1 = new Vector3(cube[index1 * 3], cube[index1 * 3 + 1], cube[index1 * 3 + 2]);
        Vector3 v2 = new Vector3(cube[index2 * 3], cube[index2 * 3 + 1], cube[index2 * 3 + 2]);
        Vector3 v3 = new Vector3(cube[index3 * 3], cube[index3 * 3 + 1], cube[index3 * 3 + 2]);
        Vector3 edge1 = v2 - v1;
        Vector3 edge2 = v3 - v1;
        Vector3 triangleNormal = Vector3.Cross(edge1, edge2);

        normalsVrs[index1] += triangleNormal;
        normalsVrs[index2] += triangleNormal;
        normalsVrs[index3] += triangleNormal;
      }

      for (int i = 0; i < normalsVrs.Length; i++) {
        normalsVrs[i] = Vector3.Normalize(normalsVrs[i]);
      }

      for (int i = 0; i < normalsVrs.Length; i += 3) {
        normals[i * 3] = normalsVrs[i].X;
        normals[i * 3 + 1] = normalsVrs[i].Y;
        normals[i * 3 + 2] = normalsVrs[i].Z;
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

    private static void GenerateIndexArray(int count, ref uint[] indices) {
      uint topInd = 0;
      uint bottomind = topInd + (uint)count;
      int ind = 0;

      for (int i = 0; i < 6; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          for (int k = 0; k < count - 2; ++k) {
            indices[ind] = topInd + 1;
            indices[ind + 1] = topInd;
            indices[ind + 2] = bottomind;
            indices[ind + 3] = topInd + 1;
            indices[ind + 4] = bottomind;
            indices[ind + 5] = bottomind + 1;
            ind += 6;
            ++topInd;
            ++bottomind;
          }
        }
      }
    }

    private static void GenerateFrozenY(int count, float step,
        ref float[] cube, ref int ind, float yCoord) {

      float currentX = -1.0f;
      float currentY = -1.0f;

      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count; ++j) {
          cube[ind] = currentX;
          cube[ind + 1] = yCoord;
          cube[ind + 2] = currentY;
          ind += 3;
          currentX += step;
        }

        currentX = -1.0f;
        currentY += step;
      }
    }

    private static void GenerateFrozenX(int count, float step,
        ref float[] cube, ref int ind, float xCoord) {
      float currentX = -1.0f;
      float currentY = -1.0f;

      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count; ++j) {
          cube[ind] = xCoord;
          cube[ind + 1] = currentX;
          cube[ind + 2] = currentY;
          ind += 3;
          currentX += step;
        }

        currentX = -1.0f;
        currentY += step;
      }
    }

    private static void GenerateFrozenZ(int count, float step,
        ref float[] cube, ref int ind, float zCoord) {
      float currentX = -1.0f;
      float currentY = -1.0f;

      for (int i = 0; i < count; ++i) {
        for (int j = 0; j < count; ++j) {
          cube[ind] = currentX;
          cube[ind + 1] = currentY;
          cube[ind + 2] = zCoord;
          ind += 3;
          currentX += step;
        }

        currentX = -1.0f;
        currentY += step;
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
