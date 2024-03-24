namespace D3D {
  // Definition of the CYaw component, which represents the yaw angle of an entity
  public class CYaw : Component {
    public float Yaw;

    // Default constructor to initialize CYaw with a default yaw angle of 0
    public CYaw() {
      Yaw = 0; 
    }

    // Constructor to initialize CYaw with a specific yaw angle
    public CYaw(float yaw) {
      Yaw = yaw; 
    }
  }
}