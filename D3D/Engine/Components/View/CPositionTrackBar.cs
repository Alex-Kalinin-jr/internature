
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class CPositionTrackBar : Component {
    public TrackBar XTrackbarObj;
    public TrackBar YTrackbarObj;
    public TrackBar ZTrackbarObj;


    public CPositionTrackBar(RenderForm form, Point position, int yStep) {
      XTrackbarObj = new TrackBar();
      XTrackbarObj.Minimum = -100;
      XTrackbarObj.Maximum = 100;
      XTrackbarObj.TickFrequency = 10;
      XTrackbarObj.LargeChange = 10;
      XTrackbarObj.Location = position;

      YTrackbarObj = new TrackBar();
      YTrackbarObj.Minimum = -100;
      YTrackbarObj.Maximum = 100;
      YTrackbarObj.TickFrequency = 10;
      YTrackbarObj.LargeChange = 10;
      YTrackbarObj.Location = new Point(position.X, position.Y + yStep);

      ZTrackbarObj = new TrackBar();
      ZTrackbarObj.Minimum = -100;
      ZTrackbarObj.Maximum = 100;
      ZTrackbarObj.TickFrequency = 10;
      ZTrackbarObj.LargeChange = 10;
      ZTrackbarObj.Location = new Point(position.X, position.Y + 2 * yStep);

      form.Controls.Add(XTrackbarObj);
      form.Controls.Add(YTrackbarObj);
      form.Controls.Add(ZTrackbarObj);
    }
  }
}
