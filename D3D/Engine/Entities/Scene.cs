using SharpDX;

namespace D3D {
  public class Scene : Entity {
    public Scene() {
      // Create imagined lights for the scene and add them as a component
      var lights = new CLight(Generator.CreateTestingPsLightConstantBuffers());
      AddComponent(lights);

      // Define the position of the camera view
      var viewPosition = new Vector3(0.0f, 0.0f, -5.0f);

      // Calculate aspect ratio (to be refactored for flexibility)
      var aspectRatio = 1024.0f / 768.0f; // Aspect ratio calculated based on width and height

      // Create a camera with the specified view position and aspect ratio and add it as a component
      var camera = new CCamera(viewPosition, aspectRatio);
      AddComponent(camera);
    }
  }
}