
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class CButton : Component {
    public Button ButtonObj;

    public CButton(RenderForm form, Point position, string text) {
      ButtonObj = new Button();
      ButtonObj.Location = position;
      ButtonObj.Size = new Size(100, 25);
      ButtonObj.Text = text;
      form.Controls.Add(ButtonObj);
    }
  }
}
