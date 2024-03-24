using System.Collections.Generic;

namespace D3D {

  public enum FigureType {
    General,
    Line,
    Pipe,
    Grid
  };

  public class DrawSystem : BaseSystem<CMesh> {
    public static List<bool> Visibility = new List<bool>();
    public static List<FigureType> Types = new List<FigureType>();

    private static Dictionary<FigureType, FigureType> _antitypes = new Dictionary<FigureType, FigureType>() {
        { FigureType.Line, FigureType.Pipe },
        { FigureType.Pipe, FigureType.Line },
    };

    public static void Register(CMesh figure, FigureType type) {
      Components.Add(figure);
      Visibility.Add(true);
      Types.Add(type);
    }

    new public static void Register(CMesh figure) {
      Components.Add(figure);
      Visibility.Add(true);
      Types.Add(FigureType.General);
    }

    new public static void Update() {
      foreach (var figure in Components) {
        int index = Components.IndexOf(figure);
        if (Visibility[index]) {
          DrawFigure(figure);
        }
      }
    }

    public static void ChangePipeType(FigureType type) {
      FigureType antiType = _antitypes[type];
      foreach (var figure in Components) {
        int ind = Components.IndexOf(figure);
        if (Types[ind] == type) {
          Visibility[ind] = true;
        } else if (Types[ind] == antiType) {
          Visibility[ind] = false;
        }
      }
    }

    public static void CliceGrid(int x, int y, int z) {
      for (int i = 0; i < Components.Count; ++i) {
// here i should adjust light buffer data
      }
    }

    public static void RestoreAllGrids() {
      for (int i = 0; i < Components.Count; ++i) {
// here i should adjust light buffer data
      }
    }

    private static void DrawFigure(CMesh figure) {
      var vertices = figure.Vertices.ToArray();
      var indices = figure.Indices.ToArray();
      var matrix = figure.TransformObj.TransformObj;
      PsLightConstantBuffer[] light = figure.EntityObj.GetComponent<CLight>().LightDataObj.ToArray();
      var topology = figure.Topology;

      var renderer = Renderer.GetRenderer();
      renderer.ChangePrimitiveTopology(topology);
      renderer.SetLightConstantBuffer(ref light);
      renderer.SetVerticesBuffer(ref vertices);
      renderer.SetIndicesBuffer(ref indices);
      renderer.SetMvpConstantBuffer(ref matrix);
      renderer.Draw(indices.Length);

    }
  }
}

