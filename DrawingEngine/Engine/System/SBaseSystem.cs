using System.Collections.Generic;

namespace D3D {
  /// <summary>
  /// Definition of the BaseSystem class, which serves as a base class for systems managing components of type T.
  /// </summary>
  /// <typeparam name="T">The type of component managed by the system.</typeparam>
  public class BaseSystem<T> where T : Component {
    /// <summary>
    /// List of components managed by the system.
    /// </summary>
    public static List<T> Components = new List<T>();

    /// <summary>
    /// Method to register a component of type T with the system.
    /// </summary>
    /// <param name="component">The component to register.</param>
    public static void Register(T component) {
      Components.Add(component);
    }

    /// <summary>
    /// Method to unregister a component of type T from the system.
    /// </summary>
    /// <param name="component">The component to unregister.</param>
    public static void Unregister(T component) {
      Components.Remove(component);
    }

    /// <summary>
    /// Method to update the system.
    /// </summary>
    public static void Update() {
      // This method can be overridden in derived classes to implement specific update logic for the system.
    }
  }
}
