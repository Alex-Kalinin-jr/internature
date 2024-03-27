using SharpDX;

namespace D3D {
  /// <summary>
  /// Represents a scene in the game world.
  /// </summary>
  public class Scene : Entity {
    /// <summary>
    /// Initializes a new instance of the <see cref="Scene"/> class.
    /// </summary>
    public Scene() {
      // Set the initial view position and aspect ratio
      var viewPosition = new Vector3(0.0f, 2.0f, -4.0f);
      var aspectRatio = 1024.0f / 768.0f; // to be refactored

      // Create a camera component and add it to the scene
      var camera = new CCamera(viewPosition, aspectRatio);
      AddComponent(camera);
    }
  }
}
