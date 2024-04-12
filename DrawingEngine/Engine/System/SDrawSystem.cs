using System.Collections.Generic;
using SharpDX;

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

    private static bool _isLineGridVisibe = false;

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
      var renderer = Renderer.GetRenderer();
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

    /// <summary>
    /// Set the property of Vertex. Property visualisation is the vertex color.
    /// </summary>
    /// <param name="type"></param>The type of property to be set. 
    /// Setting will not be perfomed if property is not presented in figure dictionary
    public static void ChangeProperty() {
      foreach (var figure in Components) {
        if (figure is CGridMesh) {
          SetProperty(ColorSystem.GetCurrentProperty(), (CGridMesh)figure);
        }
      }
    }

    public static void SetProperty(string type, CGridMesh mesh) {
      if (mesh.Properties.ContainsKey(type)) {
        int vertexCountInOneGrid = 8;
        var prop = mesh.Properties[type];
        int counter = 0;
        System.Numerics.Vector3 botCol;
        System.Numerics.Vector3 topCol;
        (botCol, topCol) = ColorSystem.GetColors();
        for (int i = 0; i < mesh.Vertices.Count; i += vertexCountInOneGrid) {
          for (int j = 0; j < vertexCountInOneGrid; ++j) {

            var val = prop[counter];
            var r = botCol.X + (topCol.X - botCol.X) * val;
            var g = botCol.Y + (topCol.Y - botCol.Y) * val;
            var b = botCol.Z + (topCol.Z - botCol.Z) * val;

            var v = mesh.Vertices[i + j];
            v.Color = new Vector3(r, g, b);
            mesh.Vertices[i + j] = v;
          }
          ++counter;
        }
      }
    }

    public static List<string> GetAllGridsProperties() {
      var list = new List<string>();
      foreach (var figure in Components) {
        if (figure is CGridMesh) {
          var figProperties = ((CGridMesh)figure).GetProperties();
          foreach (var prop in figProperties) {
            if (!list.Contains(prop)) {
              list.Add(prop);
            }
          }
        }
      }
      return list;
    }

    public static void ChangePipeAppearance(float pipeRadius, int numOfSegments) {
      for (int i = 0; i < Components.Count; ++i) {
        if (Components[i] is CPipeMesh) {
          ((CPipeMesh)Components[i]).ChangePipe(pipeRadius, numOfSegments);
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

    public static void ChangeLineGridVisibility(bool state) {
      _isLineGridVisibe = state;
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

      if (figure is CGridMesh && _isLineGridVisibe) {
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