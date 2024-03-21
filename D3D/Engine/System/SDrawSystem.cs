using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;

namespace D3D {

  public enum FigureType {
    General,
    Line,
    Pipe
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

    public static void CliceGrid(Vector3 vec) {
      foreach (var figure in Components) { 
        if (figure.GetType().Equals(typeof(CGridFigure))) {
          PrepareClicing((CGridFigure)figure, vec);
        } 
      }
    }


// it is intended that forming of indices array was perfomed in the next way:
    private static void PrepareClicing(CGridFigure figure, Vector3 vec) {
      var indCountInCube = 24;

      figure.CurrentXCount = (int)vec.X;
      figure.CurrentYCount = (int)vec.Y;
      figure.CurrentZCount = (int)vec.Z;

      int fullY = figure.YCount;
      int fullZ = figure.ZCount;

      int x = figure.CurrentXCount;
      int y = figure.CurrentYCount;
      int z = figure.CurrentZCount;

      var firstFigure = figure.FullIndices.GetRange(0, indCountInCube * fullZ * fullY * x);

      var nextFigure = new List<short>();
      for (int i = 0; i < x; ++i) {
        int start = indCountInCube * i * fullZ * fullY;
        int bias = indCountInCube * fullZ * y;
        nextFigure.AddRange(firstFigure.GetRange(start, bias));
      }

      var lastFigure = new List<short>();
      for (int i = 0; i < y * x; ++i) {
        int start = indCountInCube * i * fullZ;
        int bias = indCountInCube * z;
        lastFigure.AddRange(nextFigure.GetRange(start, bias));
      }


      figure.MeshObj.Indices = lastFigure;
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
