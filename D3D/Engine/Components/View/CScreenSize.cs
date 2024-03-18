
namespace D3D {
  public class CScreenSize : Component {
    public int Width;
    public int Height;
    
    public CScreenSize(int width, int height) {
      Width = width;
      Height = height;
    }

    public CScreenSize() {
      Width = 1024;
      Height = 768;
    }
  }

}
