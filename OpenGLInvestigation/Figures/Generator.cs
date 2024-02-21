using System;
using System.Numerics;

namespace OpenGLInvestigation.Figures {
  internal class Generator {

    public static (float[], float[], float[]) GenerateCube(int count, OpenTK.Mathematics.Vector3 color) {

      float[] cube = new float[6 * 3 * (count - 1) * (count - 1) * 6];
      float step = 2.0f / (count - 1);
      int ind = 0;



      float[] colors = new float[cube.Length];
      float[] normals = new float[cube.Length];
      GenerateFrozenX(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenX(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenZ(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenZ(count, step, ref cube, ref ind, 1.0f);
      GenerateColorArray(ref colors, color);

      GenerateNormals(ref cube, ref normals);

      return (cube, colors, normals);
    }

    private static void GenerateNormals(ref float[] vertices, ref float[] normals) {
      for (int i = 0; i < vertices.Length; i += 9) {
        Vector3 v1 = new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);
        Vector3 v2 = new Vector3(vertices[i + 3], vertices[i + 4], vertices[i + 5]);
        Vector3 v3 = new Vector3(vertices[i + 6], vertices[i + 7], vertices[i + 8]);
        Vector3 edge1 = v2 - v1;
        Vector3 edge2 = v3 - v1;
        Vector3 triangleNormal = Vector3.Cross(edge1, edge2);

        normals[i] = triangleNormal.X;
        normals[i + 1] = triangleNormal.Y;
        normals[i + 2] = triangleNormal.Z;
        normals[i + 3] = triangleNormal.X;
        normals[i + 4] = triangleNormal.Y;
        normals[i + 5] = triangleNormal.Z;
        normals[i + 6] = triangleNormal.X;
        normals[i + 7] = triangleNormal.Y;
        normals[i + 8] = triangleNormal.Z;
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

    public static void GenerateFrozenZ(int count, float step, ref float[] cube, ref int ind, float zCoord) {

      float leftX = -1.0f;
      float rightX = -1.0f + step;
      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = leftX;
          cube[ind + 1] = botY;
          cube[ind + 2] = zCoord;

          cube[ind + 3] = leftX;
          cube[ind + 4] = topY;
          cube[ind + 5] = zCoord;

          cube[ind + 6] = rightX;
          cube[ind + 7] = botY;
          cube[ind + 8] = zCoord;


          cube[ind + 9] = leftX;
          cube[ind + 10] = topY;
          cube[ind + 11] = zCoord;

          cube[ind + 12] = rightX;
          cube[ind + 13] = topY;
          cube[ind + 14] = zCoord;

          cube[ind + 15] = rightX;
          cube[ind + 16] = botY;
          cube[ind + 17] = zCoord;

          ind += 18;
          leftX += step;
          rightX += step;
        }

        rightX = -1.0f + step;
        leftX = -1.0f;
        topY += step;
        botY += step;
      }
    }

    public static void GenerateFrozenX(int count, float step, ref float[] cube, ref int ind, float xCoord) {

      float leftX = -1.0f;
      float rightX = -1.0f + step;
      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = xCoord;
          cube[ind + 1] = leftX;
          cube[ind + 2] = botY;

          cube[ind + 3] = xCoord;
          cube[ind + 4] = leftX;
          cube[ind + 5] = topY;

          cube[ind + 6] = xCoord;
          cube[ind + 7] = rightX;
          cube[ind + 8] = botY;


          cube[ind + 9] = xCoord;
          cube[ind + 10] = leftX;
          cube[ind + 11] = topY;

          cube[ind + 12] = xCoord;
          cube[ind + 13] = rightX;
          cube[ind + 14] = topY;

          cube[ind + 15] = xCoord;
          cube[ind + 16] = rightX;
          cube[ind + 17] = botY;

          ind += 18;
          leftX += step;
          rightX += step;
        }
        leftX = -1.0f;
        rightX = -1.0f + step;
        topY += step;
        botY += step;
      }
    }

    public static void GenerateFrozenY(int count, float step, ref float[] cube, ref int ind, float yCoord) {

      float leftX = -1.0f;
      float rightX = -1.0f + step;

      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = leftX;
          cube[ind + 1] = yCoord;
          cube[ind + 2] = botY;

          cube[ind + 3] = leftX;
          cube[ind + 4] = yCoord;
          cube[ind + 5] = topY;

          cube[ind + 6] = rightX;
          cube[ind + 7] = yCoord;
          cube[ind + 8] = botY;


          cube[ind + 9] = leftX;
          cube[ind + 10] = yCoord;
          cube[ind + 11] = topY;

          cube[ind + 12] = rightX;
          cube[ind + 13] = yCoord;
          cube[ind + 14] = topY;

          cube[ind + 15] = rightX;
          cube[ind + 16] = yCoord;
          cube[ind + 17] = botY;



          ind += 18;
          rightX += step;
          leftX += step;
        }

        leftX = -1.0f;
        rightX = -1.0f + step;
        topY += step;
        botY += step;
      }
    }

  }
}
