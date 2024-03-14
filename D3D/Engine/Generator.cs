using SharpDX;

namespace D3D {
// this is just debugging class. Imagine all generations you want for setting your scene, properties and so on.
  public class Generator {
    public static DataStorage CreateTestingDataStorage() {
      var output = new DataStorage();

      var figure = new Figure();
      figure.Volume = new Mesh("Resources/Tree.obj");
      float xVal = 0.0f;
      float zVal = 0.0f;
      for (int i = 0; i < 10; ++i) {
        for (int j = 0; j < 10; ++j) {
          figure.WorldMatrices.Add((ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
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
    public static Matrix ComputeTestingModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      return buff;
    }
  }

}
