using Assimp;
using SharpDX;
using System;
using System.Collections.Generic;

namespace D3D {
  // ////////////////////////////////////////////////////////////////////////////////////////////
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


  // ////////////////////////////////////////////////////////////////////////////////////////////
  public class DataStorage : IDisposable {
    public List<Figure> MeshList;
    public List<PsLightConstantBuffer> Lights;

    public DataStorage() {
      MeshList = new List<Figure>();
      Lights = new List<PsLightConstantBuffer>();
    }

    public static DataStorage CreateTestingDataStorage() {
      var output = new DataStorage();

      var figure = new Figure();
      figure.Volume = new Mesh("Resources/Tree.obj");
      float xVal = 0.0f;
      float zVal = 0.0f;
      for (int i = 0; i < 10; ++i) {
        for (int j = 0; j < 10; ++j) {
          figure.WorldMatrices.Add((ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
                                                       new Vector3(xVal, 0.0f, zVal))));
          
          xVal += 3.0f;
        }
        zVal += 3.0f;
        xVal = 0.0f;
      }
      output.MeshList.Add(figure);

      output.Lights.Add(new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f),
                                                  new Vector3(0, 0.0f, -1.0f)));
      output.Lights.Add(new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f),
                                                  new Vector3(0, 0.0f, -3.0f)));
      output.Lights.Add(new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f),
                                                  new Vector3(0, 0.0f, -4.0f)));

      return output;
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

    public static Matrix ComputeModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      return buff;
    }

    public void Dispose() {
      MeshList?.Clear();
      Lights?.Clear();
    }
  }
}
