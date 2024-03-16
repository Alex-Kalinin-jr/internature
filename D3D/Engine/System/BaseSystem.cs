using System.Collections.Generic;

namespace D3D {
  class BaseSystem<T> where T : Component {

    public static List<T> Components = new List<T>();

    public static void Register(T component) {
      Components.Add(component);
    }

    public static void Update() {
      foreach (T component in Components) {
        component.Update();
      }
    }
  }

  class TransformSystem : BaseSystem<CTransform> { }
  class CameraSystem : BaseSystem<CCamera> { }
  class WorldSystem : BaseSystem<CWorldPositions> { }
  class DrawSystem : BaseSystem<CMesh> { }
  class LightSystem : BaseSystem<CLight> { }
}
