using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace D3D {
  public class ColorSystem : DrawSystem {
    private delegate void SetVProperty(CGridMesh mesh);

    private static Vector3 _bottomColor = new Vector3(0.0f, 0.0f, 0.0f);
    private static Vector3 _topColor = new Vector3(1.0f, 1.0f, 1.0f);
    private static string _currentProperty = "color";

    private static SetVProperty Change = new SetVProperty(SetProperty);

    public static void ChangeColors(float[] bottom, float[] top) {
      var divider = 255.0f;
      _bottomColor = new Vector3(bottom[0] / divider, bottom[1] / divider, bottom[2] / divider);
      _topColor = new Vector3(top[0] / divider, top[1] / divider, top[2] / divider);
    }

    public static (Vector3, Vector3) GetColors() {
      return (_bottomColor, _topColor);
    }

    public static void ChangeProperty(string property = "") {
      if (property != "") {
        _currentProperty = property;
      }

      foreach (var figure in Components) {
        if (figure is CGridMesh) {
          Change((CGridMesh)figure);
        }
      }
    }

    public static void ChangeMethod(bool isJet) {
      if (isJet) {
        Change = InterpolateJET;
      } else {
        Change = SetProperty;
      }
    }

    private static void SetProperty(CGridMesh mesh) {
      if (mesh.Properties.ContainsKey(_currentProperty)) {
        int vertexCountInOneGrid = 8;
        var prop = mesh.Properties[_currentProperty];
        int counter = 0;
        System.Numerics.Vector3 botCol;
        System.Numerics.Vector3 topCol;
        (botCol, topCol) = GetColors();
        for (int i = 0; i < mesh.Vertices.Count; i += vertexCountInOneGrid) {
          for (int j = 0; j < vertexCountInOneGrid; ++j) {

            var val = prop[counter];
            var r = botCol.X + (topCol.X - botCol.X) * val;
            var g = botCol.Y + (topCol.Y - botCol.Y) * val;
            var b = botCol.Z + (topCol.Z - botCol.Z) * val;

            var v = mesh.Vertices[i + j];
            v.Color = new SharpDX.Vector3(r, g, b);
            mesh.Vertices[i + j] = v;
          }
          ++counter;
        }
      }
    }

    private static void InterpolateJET(CGridMesh mesh) {
      if (mesh.Properties.ContainsKey(_currentProperty)) {
        int vertexCountInOneGrid = 8;
        var prop = mesh.Properties[_currentProperty];
        int counter = 0;
        for (int i = 0; i < mesh.Vertices.Count; i += vertexCountInOneGrid) {
          for (int j = 0; j < vertexCountInOneGrid; ++j) {

            var val = prop[counter];
            float r = 0;
            float g = 0;
            float b = 0.5f + 4.0f * val;

            if (val < 0.125) {
            } else if (val < 0.375) {
              r = 0;
              g = 4.0f * (val - 0.125f);
              b = 1;
            } else if (val < 0.625) {
              r = 4 * (val - 0.375f);
              g = 1;
              b = 1 - 4 * (val - 0.375f);
            } else if (val < 0.875) {
              r = 1;
              g = 1 - 4.0f * (val - 0.125f);
              b = 0;
            } else {
              r = 1 - 4.0f * (val - 0.125f);
              g = 0;
              b = 0;
            }

            var v = mesh.Vertices[i + j];
            v.Color = new SharpDX.Vector3(r, g, b);
            mesh.Vertices[i + j] = v;
          }
          ++counter;
        }
      }
    }
  }
}
