public class TextureGenerator {
  public static void GenerateTexture() {
    string CharSheet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ";
    string MonoWidthFont = "Courier New";
    int textureWidth = 2048;
    int charCountPerRow = CharSheet.Length;
    int charWidth = textureWidth / charCountPerRow;
    int textureHeight = charWidth;

    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(textureWidth, 
        textureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
   
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
    
    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

    System.Drawing.Font font = new System.Drawing.Font(MonoWidthFont, charWidth - 1, 
        System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

    for (int i = 0; i < charCountPerRow; i++) {
      int x = i * charWidth;
      int y = 0;

      graphics.DrawString(CharSheet[i].ToString(), font, System.Drawing.Brushes.Black, new System.Drawing.PointF(x, y));
    }

    System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
    encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
    System.Drawing.Imaging.ImageCodecInfo pngCodec = GetEncoderInfo("image/png");
    bitmap.Save("char_sheet_texture.png", pngCodec, encoderParams);
  }

  private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType) {
    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
    foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs) {
      if (codec.MimeType == mimeType) {
        return codec;
      }
    }
    return null;
  }
}