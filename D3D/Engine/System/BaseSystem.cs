using System.Collections.Generic;

namespace D3D {
  public class BaseSystem<T> where T : Component {

    public static List<T> Components = new List<T>();

    public static void Register(T component) {
      Components.Add(component);
    }

    public static void Update() { }
  }

  class TransformSystem : BaseSystem<CTransform> { }
}
