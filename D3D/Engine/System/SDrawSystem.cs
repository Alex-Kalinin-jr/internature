using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;

namespace D3D {
  public class DrawSystem : BaseSystem<CFigure> {

    public enum FigureType {
      General,
      Grid,
      Pipe
    };

    public static List<bool> Visibility = new List<bool>();
    public static List<FigureType> Types = new List<FigureType>();

    private static Dictionary<FigureType, FigureType> _antitypes = new Dictionary<FigureType, FigureType>() {
        { FigureType.Grid, FigureType.Pipe },
        { FigureType.Pipe, FigureType.Grid },
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
      foreach(var figure in Components) {
        int ind = Components.IndexOf(figure);
        if (Types[ind] == type) {
          Visibility[ind] = true;
        } else if (Types[ind] == type) {
          Visibility[ind] = false;
        }
      }
    }

    private static void DrawFigure(CFigure figure) {

      var vertices = figure.IamMesh.Vertices.ToArray();
      var indices = figure.IamMesh.Indices.ToArray();
      var matrix = figure.IamTransform.IamTransform;
      var lights = figure.IamEntity.GetComponent<CLight>();
      PsLightConstantBuffer[] light = lights.IamLightData.ToArray();
      var topology = figure.IamEntity.GetComponent<CRenderParams>().IamTopology.IamTopology;
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
