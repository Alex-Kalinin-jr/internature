namespace D3D {
  /// <summary>
  /// Base class for components attached to entities.
  /// </summary>
  public class Component {
    /// <summary>
    /// The entity to which this component is attached.
    /// </summary>
    public Entity EntityObj;

    /// <summary>
    /// Updates the links between the component and its entity.
    /// </summary>
    public virtual void UpdateLinks() { }
  }
}
