namespace D3D {
  // Definition of the CMouseMovingParams class, which holds parameters related to mouse movement
  public class CMouseMovingParams {
    // Fields to hold parameters for mouse movement
    public float RotDivider = 10.0f; // Parameter for rotation divider
    public float ShiftDivider = 20.0f; // Parameter for shift divider

    // Constructor to initialize CMouseMovingParams with custom rotation and shift dividers
    public CMouseMovingParams(float iamRotDivider, float iamShiftDivider) {
      RotDivider = iamRotDivider; // Set the rotation divider
      ShiftDivider = iamShiftDivider; // Set the shift divider
    }
  }
}

