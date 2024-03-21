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

      float stepX = 1000000 / figure.XCount;
      float stepY = 1000000 / figure.YCount;
      float stepZ = 1000000 / figure.ZCount;

      int x = (int)(vec.X / stepX);
      int y = (int)(vec.Y / stepY);
      int z = (int)(vec.Z / stepZ);

      figure.CurrentXCount = x;
      figure.CurrentYCount = y;
      figure.CurrentZCount = z;

      figure.MeshObj.Indices = figure.FullIndices.GetRange(0, indCountInCube * z * y * (x - 1) - 1);
    }

    private static void DrawFigure(CFigure figure) {

      var vertices = figure.MeshObj.Vertices.ToArray();
      var indices = figure.MeshObj.Indices.ToArray();
      var matrix = figure.IamTransform.IamTransform;
      var lights = figure.IamEntity.GetComponent<CLight>();
      PsLightConstantBuffer[] light = lights.IamLightData.ToArray();
      var topology = figure.IamTopology.IamTopology;
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
