﻿using System.Collections.Generic;

namespace D3D {

  public enum FigureType {
    General,
    Line,
    Pipe,
    Grid
  };

  public class DrawSystem : BaseSystem<CMesh> {
    public static List<bool> Visibility = new List<bool>();
    static VsSliceConstantBuffer _sliceCoords = new VsSliceConstantBuffer(-1, -1, -1);

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
      FigureType antiType = _antitypes[type];
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
      _sliceCoords.Xcoord = x;
      _sliceCoords.Ycoord = y;
      _sliceCoords.Zcoord = z;
    }

    public static void RestoreAllGrids() {
      _sliceCoords.Xcoord = -1;
      _sliceCoords.Ycoord = -1;
      _sliceCoords.Zcoord = -1;
    }

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

