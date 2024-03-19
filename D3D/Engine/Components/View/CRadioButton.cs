using SharpDX.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace D3D {
  public class CRadioButton : Component {
    public RadioButton RadioButton1;
    public RadioButton RadioButton2;
    public CRadioButton(RenderForm form, Point positionOnForm) {

      RadioButton1 = new RadioButton();
      RadioButton1.Text = "Option 1";
      RadioButton1.Location = positionOnForm;

      RadioButton2 = new RadioButton();
      RadioButton2.Text = "Option 2";
      RadioButton2.Location = new Point(positionOnForm.X, positionOnForm.Y + 30);

      form.Controls.Add(RadioButton1);
      form.Controls.Add(RadioButton2);

    }

  }
}