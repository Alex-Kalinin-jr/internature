using System;
using System.Collections.Generic;
using SharpDX;

namespace D3D {
  public class Scene : Entity {

    public Figure(string filepath) {

      var volume = new CMesh(filepath);
      AddComponent(volume);

      var WorldMatrices = new CWorldPositions();
      AddComponent(WorldMatrices);


    }
  }
}
