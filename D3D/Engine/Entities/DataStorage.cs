using SharpDX;
using System;
using System.Collections.Generic;

namespace D3D {
  public class DataStorage : IDisposable {
    public List<Figure> MeshList;
    public List<PsLightConstantBuffer> Lights;

    public DataStorage() {
      MeshList = new List<Figure>();
      Lights = new List<PsLightConstantBuffer>();
    }

    public void ChangeLightColor(Vector4 color) {
      for (int i = 0; i < Lights.Count; ++i) {
        PsLightConstantBuffer light = Lights[i];
        light.Color = color;
        Lights[i] = light;
      }
    }

    public void ChangeLightDirectiron(Vector3 position) {
      for (int i = 0; i < Lights.Count; ++i) {
        PsLightConstantBuffer light = Lights[i];
        light.Position = position;
        Lights[i] = light;
      }
    }

    public void Dispose() {
      MeshList?.Clear();
      Lights?.Clear();
    }
  }
}
