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
    /// <param name="rotDivider">The rotation divider value to initialize CMouseMovingParams.</param>
    /// <param name="shiftDivider">The shift divider value to initialize CMouseMovingParams.</param>
    public CMouseMovingParams(float rotDivider, float shiftDivider) {
      RotDivider = rotDivider; // Set the rotation divider
      ShiftDivider = shiftDivider; // Set the shift divider
    }
  }
}
