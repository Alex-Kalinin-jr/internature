using System;
using System.Collections.Generic;
using System.Text;

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

}

/*
    public void ChangeLightColor(Vector4 color) {
      for (int i = 0; i < Lights.Count; ++i) {
        PsLightConstantBuffer light = Lights[i];
        light.Color = color;
        Lights[i] = light;
      }
    }

    public void ChangeLightDirectiron(Vector3 position) {
      for (int i = 0; i < Lights.Count; ++i) {
        PsLightConstantBuffer light = Lights[i];
        light.Position = position;
        Lights[i] = light;
      }
 */