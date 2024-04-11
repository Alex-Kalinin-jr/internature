namespace D3D {
  /// <summary>
  /// Definition of the CScreenSize component, which represents the screen size.
  /// </summary>
  public class CScreenSize : Component {
    /// <summary>
    /// The screen width.
    /// </summary>
    public int Width;

    /// <summary>
    /// The screen height.
    /// </summary>
    public int Height;

    /// <summary>
    /// Constructor to initialize CScreenSize with custom width and height.
    /// </summary>
    /// <param name="width">The width value to initialize CScreenSize.</param>
    /// <param name="height">The height value to initialize CScreenSize.</param>
    public CScreenSize(int width, int height) {
      Width = width;
      Height = height;
    }

    /// <summary>
    /// Default constructor to initialize CScreenSize with default width and height 1024x768.
    /// </summary>
    public CScreenSize() {
      Width = 1024;
      Height = 768;
    }
  }
}
