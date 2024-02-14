using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app {
  internal class Generator {

    public static (float[], uint[]) GenerateCube(int count) {

      float[] cube = new float[3 * (count) * (count) * 6];
      uint[] idxs = new uint[(count - 1) * (count - 1) * 2 * 6 * 3];

      float step = 2.0f / (float)(count - 1);
      int ind = 0;

      GenerateFrozenX(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenX(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, -1.0f);
      GenerateFrozenY(count, step, ref cube, ref ind, 1.0f);
      GenerateFrozenZ(count, step, ref  cube, ref ind, -1.0f);
      GenerateFrozenZ(count, step, ref cube, ref ind, 1.0f);

      GenerateIndexArray(count, ref idxs);

      return (cube, idxs);
    }

    private static void GenerateIndexArray(int count, ref uint[] indices) {
      uint topInd = 0;
      uint bottomind = topInd + (uint)count;
      int ind = 0;

      for (int i = 0; i < 6; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          for (int k = 0; k < count - 2; ++k) {
            indices[ind] = topInd;
            indices[ind + 1] = topInd + 1;
            indices[ind + 2] = bottomind;
            indices[ind + 3] = bottomind;
            indices[ind + 4] = topInd + 1;
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
