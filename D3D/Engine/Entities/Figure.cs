using System;
using System.Collections.Generic;
using SharpDX;

namespace D3D {
  public class Figure : IDisposable {
    public Mesh Volume;
    public List<Matrix> WorldMatrices;

    public Figure() {
      WorldMatrices = new List<Matrix>();
    }

    public void Dispose() {
      WorldMatrices.Clear();
    }
  }
}
