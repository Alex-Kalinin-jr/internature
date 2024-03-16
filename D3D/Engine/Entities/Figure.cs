using System;
using System.Collections.Generic;
using SharpDX;

namespace D3D {
  public class Scene : Entity {

    public Scene(string filepath, List<Matrix> worldMatrices) {

      var volume = new CMesh(filepath);
      AddComponent(volume);

      var WorldMatrices = new CWorldPositions(worldMatrices);
      AddComponent(WorldMatrices);

      var lights = new CLight(Generator.CreateTestingPsLightConstantBuffers());
      AddComponent(lights);

      var camera = new CCamera();
      AddComponent(camera);
    }
  }
}
