namespace D3D {
  /// <summary>
  /// Definition of the CPitch component, which represents the pitch angle of an entity.
  /// </summary>
  public class CPitch : Component {
    /// <summary>
    /// Field to hold the pitch angle.
    /// </summary>
    public float Pitch;

    /// <summary>
    /// Constructor to initialize CPitch with a specific pitch angle.
    /// </summary>
    /// <param name="pitch">The pitch angle to initialize CPitch.</param>
    public CPitch(float pitch) {
      Pitch = pitch;
    }

    /// <summary>
    /// Default constructor to initialize CPitch with a default pitch angle of 0.
    /// </summary>
    public CPitch() {
      Pitch = 0;
    }
  }
}


