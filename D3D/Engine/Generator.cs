using SharpDX;
using SharpDX.DXGI;
using System.Collections.Generic;

namespace D3D {
// this is just debugging class. Imagine all generations you want for setting your scene, properties and so on.
  public class Generator {

    public static Scene CreateTestingScene() {
      List<Matrix> matrices = new List<Matrix>();
      float xVal = 0.0f;
      float zVal = 0.0f;
      for (int i = 0; i < 3; ++i) {
        for (int j = 0; j < 3; ++j) {
          matrices.Add((ComputeTestingModelMatrix(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(xVal, 0.0f, zVal))));

          xVal += 4.0f;
        }
        zVal += 4.0f;
        xVal = 0.0f;
      }
      return new Scene("Resources/Tree.obj", matrices);
    }

    public static List<PsLightConstantBuffer> CreateTestingPsLightConstantBuffers() {
      var output = new List<PsLightConstantBuffer> {
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -1.0f)),
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -3.0f)),
        new PsLightConstantBuffer(new Vector4(0.0f, 0.6f, 0.0f, 1.0f), new Vector3(0, 0.0f, -4.0f))
      };
      return output;
    }

    public static Matrix ComputeTestingModelMatrix(Vector3 rotations, Vector3 translations) {
      var buff = Matrix.RotationYawPitchRoll(rotations.X, rotations.Y, rotations.Z) * Matrix.Translation(translations);
      return buff;
    }
  }

}
