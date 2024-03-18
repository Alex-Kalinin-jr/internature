using SharpDX.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace D3D {
  public class CLabel : Component {
    public CLabel(RenderForm form, Point positionOnForm, string text) {

      Label _labelHelp = new Label();
      _labelHelp.AutoSize = true;
      _labelHelp.Location = positionOnForm;
      _labelHelp.Name = "help";
      _labelHelp.TabIndex = 0;
      _labelHelp.Text = text;
      _labelHelp.TextAlign = ContentAlignment.MiddleLeft;

// to be replaced into system
      form.Controls.Add(_labelHelp);

    }

  }
}
