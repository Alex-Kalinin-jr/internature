using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;

namespace D3D {

  public enum FigureType {
    General,
    Line,
    Pipe,
    Grid
  };

  public class DrawSystem : BaseSystem<CMesh> {
    public static List<bool> Visibility = new List<bool>();
    public static int slice = -1;

    private static Dictionary<FigureType, FigureType> _antitypes = new Dictionary<FigureType, FigureType>() {
        { FigureType.Line, FigureType.Pipe },
        { FigureType.Pipe, FigureType.Line },
    };

    new public static void Register(CMesh figure) {
      Components.Add(figure);
      Visibility.Add(true);
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
        if (figure.FigureTypeObj == type) {
          Visibility[ind] = true;
        } else if (figure.FigureTypeObj == antyType) {
          Visibility[ind] = false;
        }
      }
    }

    public static void CliceGrid(int x, int y, int z) {
      for (int i = 0; i < Components.Count; ++i) {
// here
      }
    }

    public static void RestoreAllGrids() {
      for (int i = 0; i < Components.Count; ++i) {
// here
      }
    }



    // it is intended that forming of indices array was perfomed in the next way:
    private static void PrepareClicing(CMesh figure, Vector3 vec) {
// here
    }

    private static void DrawFigure(CMesh figure) {

      var vertices = figure.Vertices.ToArray();
      var indices = figure.Indices.ToArray();
      var matrix = figure.TransformObj.TransformObj;
      var lights = figure.EntityObj.GetComponent<CLight>();
      PsLightConstantBuffer[] light = lights.LightDataObj.ToArray();
      var topology = figure.TopologyObj;



      var renderer = Renderer.GetRenderer();
      renderer.ChangePrimitiveTopology(topology); 
      renderer.SetLightConstantBuffer(ref light);
      renderer.SetVerticesBuffer(ref vertices);
      renderer.SetIndicesBuffer(ref indices);
      renderer.SetMvpConstantBuffer(ref matrix);
      VsSliceConstantBuffer buff = new VsSliceConstantBuffer(3, 2, 3);
      renderer.SetSliceConstantBuffer(ref buff);
      renderer.Draw(indices.Length);

    }
  }
}
