using System.Collections.Generic;

namespace D3D {
  // Definition of the BaseSystem class, which serves as a base class for systems managing components of type T
  public class BaseSystem<T> where T : Component {
    public static List<T> Components = new List<T>();

    // Method to register a component of type T with the system
    public static void Register(T component) {
      Components.Add(component); 
    }

    // Method to update the system (empty implementation, to be overridden in derived classes)
    public static void Update() {
      // This method can be overridden in derived classes to implement specific update logic for the system
    }
  }
}