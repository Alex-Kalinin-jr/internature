using System.Drawing;
using System.Drawing.Imaging;

public class TextureGenerator {
  static string CharSheet = "a_2";
  static string MonoWidthFont = "Courier New"; // Monospaced font

  public static void GenerateTexture() {
    int textureWidth = 48; // Width of the texture
    int charCountPerRow = CharSheet.Length;
    int charWidth = textureWidth / charCountPerRow; // Calculate the character width based on the texture width and character count
    int textureHeight = charWidth; // Assume square texture for monospaced characters

    Bitmap bitmap = new Bitmap(textureWidth, textureHeight);
    Graphics graphics = Graphics.FromImage(bitmap);
    Font font = new Font(MonoWidthFont, charWidth, FontStyle.Bold, GraphicsUnit.Pixel); // Specify the monospaced font

    for (int i = 0; i < charCountPerRow; i++) {
      int x = i * charWidth;
      int y = 0;

      graphics.DrawString(CharSheet[i].ToString(), font, Brushes.White, new PointF(x, y));
    }

    bitmap.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipXY);
    bitmap.Save("a_2.png", ImageFormat.Png);
    // Save the bitmap as a PNG file
  }
}