using System.Collections.Generic;

namespace D3D {
  /// <summary>
  /// Represents an entity in the game world.
  /// </summary>
  public class Entity {
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public int Id { get; set; }

    // Private field to hold a list of components attached to the entity
    private List<Component> _components = new List<Component>();

    /// <summary>
    /// Adds a component to the entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public void AddComponent(Component component) {
      component.EntityObj = this;
      component.UpdateLinks();
      _components.Add(component);
    }

    /// <summary>
    /// Removes a component of type T from the entity.
    /// </summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    public void RemoveComponent<T>() {
      foreach (Component component in _components) {
        if (component.GetType().Equals(typeof(T))) {
          _components.Remove(component);
          return; // Exit the loop after removing the first occurrence
        }
      }
    }

    /// <summary>
    /// Retrieves a component of type T attached to the entity.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <returns>The component of type T, or null if not found.</returns>
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
