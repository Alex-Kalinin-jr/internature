namespace D3D {
  // Definition of the CPitch component, which represents the pitch angle of an entity
  public class CPitch : Component {
    public float Pitch; // Field to hold the pitch angle

    // Constructor to initialize CPitch with a specific pitch angle
    public CPitch(float pitch) {
      Pitch = pitch;
    }

    // Default constructor to initialize CPitch with a default pitch angle of 0
    public CPitch() {
      Pitch = 0; 
    }
  }
}

