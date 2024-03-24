using SharpDX;

namespace D3D {
  public class Scene : Entity {
    public Scene() {

      var lights = new CLight(Generator.CreateTestingPsLightConstantBuffers());
      AddComponent(lights);

      var viewPosition = new Vector3(20.0f, 5.0f, -5.0f);
      var aspectRatio = 1024.0f / 768.0f; // to be refactored
      var camera = new CCamera(viewPosition, aspectRatio);
      AddComponent(camera);

    }
  }
}
