namespace D3D {
  /// <summary>
  /// Definition of the CMouseMovingParams class, which holds parameters related to mouse movement.
  /// </summary>
  public class CMouseMovingParams {
    /// <summary>
    /// Parameter for rotation divider.
    /// </summary>
    public float RotDivider = 10.0f;

    /// <summary>
    /// Parameter for shift divider.
    /// </summary>
    public float ShiftDivider = 20.0f;

    /// <summary>
    /// Constructor to initialize CMouseMovingParams with custom rotation and shift dividers.
    /// </summary>
    /// <param name="iamRotDivider">The rotation divider value to initialize CMouseMovingParams.</param>
    /// <param name="iamShiftDivider">The shift divider value to initialize CMouseMovingParams.</param>
    public CMouseMovingParams(float iamRotDivider, float iamShiftDivider) {
      RotDivider = iamRotDivider; // Set the rotation divider
      ShiftDivider = iamShiftDivider; // Set the shift divider
    }
  }
}
