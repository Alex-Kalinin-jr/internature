using System;
using System.Collections.Generic;
using System.Text;

namespace D3D {
  class BaseSystem<T> where T : Component {

    public static List<T> Components = new List<T>();

    public static void Register(T component) {
      Components.Add(component);
    }

    public static void Update(float gameTime) {
      foreach (T component in Components) {
        component.Update();
      }
    }
  }

  class TransformSystem : BaseSystem<CTransform> { }
  class CameraSystem : BaseSystem<CCamera> { }
  class DrawingSystem : BaseSystem<Figure> { }
  // intended for upgrading world matrices
  class WorldSystem : BaseSystem<CWorldPositions> { }

}