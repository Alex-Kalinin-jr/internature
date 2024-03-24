namespace D3D {
  // Definition of the CScreenSize component, which represents the screen size
  public class CScreenSize : Component {
    // Fields to hold screen width and height
    public int Width; 
    public int Height; 

    // Constructor to initialize CScreenSize with custom width and height
    public CScreenSize(int width, int height) {
      Width = width; 
      Height = height; 
    }

    // Default constructor to initialize CScreenSize with default width and height  1024 x 768
    public CScreenSize() {
      Width = 1024; 
      Height = 768; 
    }
  }
}