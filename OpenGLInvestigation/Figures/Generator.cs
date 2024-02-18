﻿using System;
using System.Numerics;

namespace OpenGLInvestigation.Figures {
  internal class Generator {

    public static (float[], uint[], float[]) GenerateCube(int count, OpenTK.Mathematics.Vector3 color) {

      float[] cube = new float[3 * count * count * 6];
      uint[] idxs = new uint[(count - 1) * (count - 1) * 2 * 6 * 3];
      float[] colors = new float[cube.Length];

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

      return (cube, idxs, colors);
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

  }
}