using System.Collections.Generic;

namespace D3D {
  /// <summary>
  /// Enumeration for different types of figures.
  /// </summary>
  public enum FigureType {
    General,
    Line,
    Pipe,
    Grid
  };

  /// <summary>
  /// Class representing the drawing system.
  /// </summary>
  public class DrawSystem : BaseSystem<CMesh> {
    /// <summary>
    /// List to store the visibility status of each figure.
    /// </summary>
    public static List<bool> Visibility = new List<bool>();

    /// <summary>
    /// Constant buffer for slice coordinates.
    /// </summary>
    static VsSliceConstantBuffer _sliceCoords = new VsSliceConstantBuffer(-1, -1, -1);

    /// <summary>
    /// Dictionary to store antitypes of figure types.
    /// </summary>
    private static Dictionary<FigureType, FigureType> _antitypes = new Dictionary<FigureType, FigureType>()
    {
            { FigureType.Line, FigureType.Pipe },
            { FigureType.Pipe, FigureType.Line },
        };

    /// <summary>
    /// Method to register a figure in the drawing system.
    /// </summary>
    /// <param name="figure">The figure to register.</param>
    new public static void Register(CMesh figure) {
      Components.Add(figure);
      Visibility.Add(true);
    }

    /// <summary>
    /// Method to update the drawing system.
    /// </summary>
    new public static void Update() {
      foreach (var figure in Components) {
        int index = Components.IndexOf(figure);
        if (Visibility[index]) {
          DrawFigure(figure);
        }
      }
    }

    /// <summary>
    /// Method to change the type of pipe figures.
    /// </summary>
    /// <param name="type">The new type for pipe figures.</param>
    public static void ChangePipeType(FigureType type) {
      FigureType antiType = _antitypes[type];
      foreach (var figure in Components) {
        int ind = Components.IndexOf(figure);
        if (figure.FigureTypeObj == type) {
          Visibility[ind] = true;
        } else if (figure.FigureTypeObj == antiType) {
          Visibility[ind] = false;
        }
      }
    }

    /// <summary>
    /// Method to slice the grid at specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate for slicing.</param>
    /// <param name="y">The Y coordinate for slicing.</param>
    /// <param name="z">The Z coordinate for slicing.</param>
    public static void CliceGrid(int x, int y, int z) {
      _sliceCoords.Xcoord = x;
      _sliceCoords.Ycoord = y;
      _sliceCoords.Zcoord = z;
    }


    public static void ChangeProperty(CGridMesh.PropertyType type) {
      foreach (var figure in Components) {
        if (figure is CGridMesh) {
          ((CGridMesh)figure).SetProperty(type);
        }
      }
    }


    /// <summary>
    /// Method to restore all grid slices.
    /// </summary>
    public static void RestoreAllGrids() {
      _sliceCoords.Xcoord = -1;
      _sliceCoords.Ycoord = -1;
      _sliceCoords.Zcoord = -1;
    }

    /// <summary>
    /// Method to draw a figure.
    /// </summary>
    /// <param name="figure">The figure to draw.</param>
    private static void DrawFigure(CMesh figure) {
      var vertices = figure.Vertices.ToArray();
      var indices = figure.Indices.ToArray();
      var matrix = figure.TransformObj.TransformObj;
      var topology = figure.TopologyObj;

      var renderer = Renderer.GetRenderer();

      renderer.SetVerticesBuffer(ref vertices);
      renderer.SetMvpConstantBuffer(ref matrix);
      renderer.ChangePrimitiveTopology(topology);
      renderer.SetIndicesBuffer(ref indices);
      renderer.SetSliceConstantBuffer(ref _sliceCoords);
      renderer.Draw(indices.Length);

      if (figure is CGridMesh) {
        _sliceCoords.Bias = 0;
        var lineIndices = ((CGridMesh)figure).LineIndices.ToArray();
        renderer.ChangePrimitiveTopology(SharpDX.Direct3D.PrimitiveTopology.LineList);
        renderer.SetIndicesBuffer(ref lineIndices);
        renderer.SetSliceConstantBuffer(ref _sliceCoords);
        renderer.Draw(lineIndices.Length);
        _sliceCoords.Bias = -1;
      }
    }
  }
}
