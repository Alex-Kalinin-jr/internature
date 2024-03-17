using System.Collections.Generic;
using SharpDX;

namespace D3D {
  public class Scene : Entity {

    public Scene( ) {

      var lights = new CLight(Generator.CreateTestingPsLightConstantBuffers());
      AddComponent(lights);

      var camera = new CCamera();
      camera.IamCamera = new Camera(new Vector3(0.0f, 1.0f, 3.0f), 1024.0f / 768.0f);
      AddComponent(camera);

    }
  }
}
