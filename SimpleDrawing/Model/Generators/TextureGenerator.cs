﻿namespace SimpleDrawing.Model {

  public class TextureGenerator {
    public static void GenerateTexture(string charSheet, string fontType, int textureWidth) {

      try {
        System.Drawing.Font font = new System.Drawing.Font(fontType, textureWidth,
          System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

        int textureHeight = textureWidth / 2 * 3; // aspect ratio is choosen 3 : 2

        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(textureWidth,
            textureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;



        for (int i = 0; i < charSheet.Length; i++) {
          graphics.Clear(System.Drawing.Color.Black);

          graphics.DrawString(charSheet[i].ToString(),
              font, System.Drawing.Brushes.White, new System.Drawing.PointF(0.0f, 0.0f));

          System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);

          encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

          System.Drawing.Imaging.ImageCodecInfo pngCodec = GetEncoderInfo("image/png");

          bitmap.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

          bitmap.Save(charSheet[i].ToString() + ".png", pngCodec, encoderParams);
        }
      } catch (Exception ex) {
        Console.WriteLine("Font generation error: " + ex.Message);
      }

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
}