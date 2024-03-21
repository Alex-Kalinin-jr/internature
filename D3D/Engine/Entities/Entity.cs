using System.Collections.Generic;


namespace D3D {
  public class Entity {
    public int Id { get; set; }
    List<Component> _components = new List<Component>();

    public void AddComponent(Component component) {
      _components.Add(component);
      component.EntityObj = this;
      component.UpdateLinks();
    }

    public void RemoveComponent<T>() {
      foreach (Component component in _components) {
        if (component.GetType().Equals(typeof(T))) {
          _components.Remove(component);
          return;
        }
      }
    }

    public T GetComponent<T>() where T : Component {
      foreach (Component component in _components) {
        if (component.GetType().Equals(typeof(T))) {
          return (T)component;
        }
      }
      return null;
    }
  }
}
