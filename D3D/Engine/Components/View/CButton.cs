
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class CButton {
    public Button IamButton;

    public CButton(RenderForm form, Point position, string text) {
      IamButton = new Button();
      IamButton.Location = position;
      IamButton.Size = new Size(100, 25);
      IamButton.Text = text;
      form.Controls.Add(IamButton);
    }
  }
}
