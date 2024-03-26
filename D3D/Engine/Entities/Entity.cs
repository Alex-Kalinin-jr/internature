using System.Collections.Generic;

namespace D3D {
  public class Entity {
    public int Id { get; set; }

    // Private field to hold a list of components attached to the entity
    List<Component> _components = new List<Component>();


    public void AddComponent(Component component) {
      component.EntityObj = this;
      component.UpdateLinks();
      _components.Add(component);
    }

    // Method to remove a component of type T from the entity
    public void RemoveComponent<T>() {
      foreach (Component component in _components) {
        if (component.GetType().Equals(typeof(T))) { 
          _components.Remove(component); 
          return; // Exit the loop after removing the first occurrence
        }
      }
    }

    public T GetComponent<T>() where T : Component {
      foreach (Component component in _components) {
        if (component.GetType().Equals(typeof(T))) { 
          return (T)component; // Return the first component casted to type T
        }
      }
      return null;
    }
  }
}