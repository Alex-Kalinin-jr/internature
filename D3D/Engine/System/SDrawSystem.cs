using System.Collections.Generic;

namespace D3D {

  public enum FigureType {
    General,
    Line,
    Pipe,
    Grid
  };

  public class DrawSystem : BaseSystem<CFigure> {
    public static List<bool> Visibility = new List<bool>();
    public static List<FigureType> Types = new List<FigureType>();

    private static Dictionary<FigureType, FigureType> _antitypes = new Dictionary<FigureType, FigureType>() {
        { FigureType.Line, FigureType.Pipe },
        { FigureType.Pipe, FigureType.Line },
    };

    public static void Register(CFigure figure, FigureType type) {
      Components.Add(figure);
      Visibility.Add(true);
      Types.Add(type);
    }

    new public static void Register(CFigure figure) {
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
      FigureType antyType = _antitypes[type];
      foreach (var figure in Components) {
        int ind = Components.IndexOf(figure);
        if (Types[ind] == type) {
          Visibility[ind] = true;
        } else if (Types[ind] == antyType) {
          Visibility[ind] = false;
        }
      }
    }

    public static void CliceGrid(int x, int y, int z) {
      for (int i = 0; i < Components.Count; ++i) {
        if (Components[i].GetType().Equals(typeof(CNewGridFigure))) {
          var buff = (CNewGridFigure)Components[i];
          if (buff.XCoord != x && buff.YCoord != y && buff.ZCoord != z) {
            Visibility[i] = false;
          } else {
            Visibility[i] = true;
          }
        }
      }
    }

    public static void RestoreAllGrids() {
      for (int i = 0; i < Components.Count; ++i) {
        if (Components[i].GetType().Equals(typeof(CNewGridFigure))) {
            Visibility[i] = true;
        }
      }
    }

    private static void DrawFigure(CFigure figure) {

      var vertices = figure.MeshObj.Vertices.ToArray();
      var indices = figure.MeshObj.Indices.ToArray();
      var matrix = figure.TransformObj.TransformObj;
      var lights = figure.EntityObj.GetComponent<CLight>();
      PsLightConstantBuffer[] light = lights.LightDataObj.ToArray();
      var topology = figure.TopologyObj.TopologyObj;

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
