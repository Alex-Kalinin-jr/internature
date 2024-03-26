namespace D3D {
  /// <summary>
  /// Definition of the CYaw component, which represents the yaw angle of an entity.
  /// </summary>
  public class CYaw : Component {
    /// <summary>
    /// Field to hold the yaw angle.
    /// </summary>
    public float Yaw;

    /// <summary>
    /// Default constructor to initialize CYaw with a default yaw angle of 0.
    /// </summary>
    public CYaw() {
      Yaw = 0;
    }

    /// <summary>
    /// Constructor to initialize CYaw with a specific yaw angle.
    /// </summary>
    /// <param name="yaw">The yaw angle to initialize CYaw.</param>
    public CYaw(float yaw) {
      Yaw = yaw;
    }
  }
}
