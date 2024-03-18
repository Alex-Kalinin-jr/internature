
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class CPositionTrackBar {
    public TrackBar IamTrackBar;
    public CPositionTrackBar(RenderForm form, Point position) {
      IamTrackBar = new TrackBar();
      IamTrackBar.Minimum = -100;
      IamTrackBar.Maximum = 100;
      IamTrackBar.TickFrequency = 10;
      IamTrackBar.LargeChange = 10;
      IamTrackBar.Location = position;
      form.Controls.Add(IamTrackBar);
    }
  }
}
