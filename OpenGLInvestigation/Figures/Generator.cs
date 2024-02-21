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
      /*
      GenerateNormals(ref cube, ref idxs, ref normals);
      */
      return (cube, colors, normals);
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

      float botLeftX = -1.0f;
      float botRightX = -1.0f + step;
      float topLeftX = -1.0f;
      float topRightX = -1.0f + step;

      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = botLeftX;
          cube[ind + 1] = botY;
          cube[ind + 2] = zCoord;

          cube[ind + 3] = botRightX;
          cube[ind + 4] = botY;
          cube[ind + 5] = zCoord;

          cube[ind + 6] = topLeftX;
          cube[ind + 7] = topY;
          cube[ind + 8] = zCoord;

          cube[ind + 9] = botLeftX;
          cube[ind + 10] = botY;
          cube[ind + 11] = zCoord;

          cube[ind + 12] = botRightX;
          cube[ind + 13] = botY;
          cube[ind + 14] = zCoord;

          cube[ind + 15] = topRightX;
          cube[ind + 16] = topY;
          cube[ind + 17] = zCoord;

          ind += 18;
          topLeftX += step;
          topRightX += step;
          botLeftX += step;
          botRightX += step;
        }

        topLeftX = -1.0f;
        topRightX = -1.0f + step;
        botLeftX = -1.0f;
        botRightX = -1.0f + step;
        topY += step;
        botY += step;
      }
    }

    public static void GenerateFrozenX(int count, float step, ref float[] cube, ref int ind, float xCoord) {

      float botLeftX = -1.0f;
      float botRightX = -1.0f + step;
      float topLeftX = -1.0f;
      float topRightX = -1.0f + step;

      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = xCoord;
          cube[ind + 1] = botLeftX;
          cube[ind + 2] = botY;

          cube[ind + 3] = xCoord;
          cube[ind + 4] = botRightX;
          cube[ind + 5] = botY;

          cube[ind + 6] = xCoord;
          cube[ind + 7] = topLeftX;
          cube[ind + 8] = topY;

          cube[ind + 9] = xCoord;
          cube[ind + 10] = botLeftX;
          cube[ind + 11] = botY;

          cube[ind + 12] = xCoord;
          cube[ind + 13] = botRightX;
          cube[ind + 14] = botY;

          cube[ind + 15] = xCoord;
          cube[ind + 16] = topRightX;
          cube[ind + 17] = topY;

          ind += 18;
          topLeftX += step;
          topRightX += step;
          botLeftX += step;
          botRightX += step;
        }

        topLeftX = -1.0f;
        topRightX = -1.0f + step;
        botLeftX = -1.0f;
        botRightX = -1.0f + step;
        topY += step;
        botY += step;
      }
    }

    public static void GenerateFrozenY(int count, float step, ref float[] cube, ref int ind, float yCoord) {

      float botLeftX = -1.0f;
      float botRightX = -1.0f + step;
      float topLeftX = -1.0f;
      float topRightX = -1.0f + step;

      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = botLeftX;
          cube[ind + 1] = yCoord;
          cube[ind + 2] = botY;

          cube[ind + 3] = botRightX;
          cube[ind + 4] = yCoord;
          cube[ind + 5] = botY;

          cube[ind + 6] = topLeftX;
          cube[ind + 7] = yCoord;
          cube[ind + 8] = topY;

          cube[ind + 9] = botLeftX;
          cube[ind + 10] = yCoord;
          cube[ind + 11] = botY;

          cube[ind + 12] = botRightX;
          cube[ind + 13] = yCoord;
          cube[ind + 14] = botY;

          cube[ind + 15] = topRightX;
          cube[ind + 16] = yCoord;
          cube[ind + 17] = topY;

          ind += 18;
          topLeftX += step;
          topRightX += step;
          botLeftX += step;
          botRightX += step;
        }

        topLeftX = -1.0f;
        topRightX = -1.0f + step;
        botLeftX = -1.0f;
        botRightX = -1.0f + step;
        topY += step;
        botY += step;
      }
    }




  }
}
