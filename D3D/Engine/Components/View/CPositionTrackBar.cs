
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class CPositionTrackBar : Component {
    public TrackBar IamXTrackBar;
    public TrackBar IamYTrackBar;
    public TrackBar IamZTrackBar;


    public CPositionTrackBar(RenderForm form, Point position, int yStep) {
      IamXTrackBar = new TrackBar();
      IamXTrackBar.Minimum = -100;
      IamXTrackBar.Maximum = 100;
      IamXTrackBar.TickFrequency = 10;
      IamXTrackBar.LargeChange = 10;
      IamXTrackBar.Location = position;

      IamYTrackBar = new TrackBar();
      IamYTrackBar.Minimum = -100;
      IamYTrackBar.Maximum = 100;
      IamYTrackBar.TickFrequency = 10;
      IamYTrackBar.LargeChange = 10;
      IamYTrackBar.Location = new Point(position.X, position.Y + yStep);

      IamZTrackBar = new TrackBar();
      IamZTrackBar.Minimum = -100;
      IamZTrackBar.Maximum = 100;
      IamZTrackBar.TickFrequency = 10;
      IamZTrackBar.LargeChange = 10;
      IamZTrackBar.Location = new Point(position.X, position.Y + 2 * yStep);

      form.Controls.Add(IamXTrackBar);
      form.Controls.Add(IamYTrackBar);
      form.Controls.Add(IamZTrackBar);
    }
  }
}
