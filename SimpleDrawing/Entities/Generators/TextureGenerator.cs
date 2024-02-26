using System.Drawing.Imaging;
using System.Drawing;

public class TextureGenerator {
  public static void GenerateTexture() {
    string CharSheet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ";
    string MonoWidthFont = "Courier New";
    int textureWidth = 2048;
    int charCountPerRow = CharSheet.Length;
    int charWidth = textureWidth / charCountPerRow;
    int textureHeight = charWidth;

    Bitmap bitmap = new Bitmap(textureWidth, textureHeight, PixelFormat.Format32bppArgb);
    Graphics graphics = Graphics.FromImage(bitmap);
    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
    Font font = new Font(MonoWidthFont, charWidth - 1, FontStyle.Regular, GraphicsUnit.Pixel);

    for (int i = 0; i < charCountPerRow; i++) {
      int x = i * charWidth;
      int y = 0;

      graphics.DrawString(CharSheet[i].ToString(), font, Brushes.Black, new PointF(x, y));
    }

    EncoderParameters encoderParams = new EncoderParameters(1);
    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
    ImageCodecInfo pngCodec = GetEncoderInfo("image/png");
    bitmap.Save("char_sheet_texture.png", pngCodec, encoderParams);
  }

  private static ImageCodecInfo GetEncoderInfo(string mimeType) {
    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
    foreach (ImageCodecInfo codec in codecs) {
      if (codec.MimeType == mimeType) {
        return codec;
      }
    }
    return null;
  }
}