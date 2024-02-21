﻿using System;
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

      float leftX = -1.0f;
      float rightX = -1.0f + step;
      float botY = -1.0f;
      float topY = -1.0f + step;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = leftX;
          cube[ind + 1] = botY;
          cube[ind + 2] = zCoord;

          cube[ind + 3] = rightX;
          cube[ind + 4] = botY;
          cube[ind + 5] = zCoord;

          cube[ind + 6] = leftX;
          cube[ind + 7] = topY;
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
          cube[ind + 4] = rightX;
          cube[ind + 5] = botY;

          cube[ind + 6] = xCoord;
          cube[ind + 7] = leftX;
          cube[ind + 8] = topY;

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

          cube[ind + 3] = rightX;
          cube[ind + 4] = yCoord;
          cube[ind + 5] = botY;

          cube[ind + 6] = leftX;
          cube[ind + 7] = yCoord;
          cube[ind + 8] = topY;

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
