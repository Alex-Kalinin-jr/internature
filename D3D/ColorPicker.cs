using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace D3D {
  class ColorRangePicker : Form {
    private NumericUpDown bottomRedNumericUpDown;
    private NumericUpDown bottomGreenNumericUpDown;
    private NumericUpDown bottomBlueNumericUpDown;
    private NumericUpDown topRedNumericUpDown;
    private NumericUpDown topGreenNumericUpDown;
    private NumericUpDown topBlueNumericUpDown;
    private Label colorLabel;

    public ColorRangePicker() {
      bottomRedNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(50, 50)
      };

      bottomGreenNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(110, 50)
      };

      bottomBlueNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(170, 50)
      };

      topRedNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(50, 100)
      };

      topGreenNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(110, 100)
      };

      topBlueNumericUpDown = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(170, 100)
      };

      colorLabel = new Label {
        Text = "Selected Color Range: Black - White",
        Location = new Point(50, 150)
      };

      bottomRedNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      bottomGreenNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      bottomBlueNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      topRedNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      topGreenNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      topBlueNumericUpDown.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      Controls.Add(bottomRedNumericUpDown);
      Controls.Add(bottomGreenNumericUpDown);
      Controls.Add(bottomBlueNumericUpDown);
      Controls.Add(topRedNumericUpDown);
      Controls.Add(topGreenNumericUpDown);
      Controls.Add(topBlueNumericUpDown);
      Controls.Add(colorLabel);

      Paint += (sender, e) => {
        Rectangle gradientRect = new Rectangle(50, 200, 200, 50);
        using (LinearGradientBrush brush = new LinearGradientBrush(gradientRect,
                                                                   Color.FromArgb((int)bottomRedNumericUpDown.Value,
                                                                   (int)bottomGreenNumericUpDown.Value,
                                                                   (int)bottomBlueNumericUpDown.Value),
                                                                   Color.FromArgb((int)topRedNumericUpDown.Value,
                                                                   (int)topGreenNumericUpDown.Value,
                                                                   (int)topBlueNumericUpDown.Value),
                                                                   LinearGradientMode.Vertical)) {
          e.Graphics.FillRectangle(brush, gradientRect);
        }
      };

    }

    private void UpdateColor() {

      Color startColor = Color.FromArgb((int)bottomRedNumericUpDown.Value,
                                        (int)bottomGreenNumericUpDown.Value,
                                        (int)bottomBlueNumericUpDown.Value);
      Color endColor = Color.FromArgb((int)topRedNumericUpDown.Value,
                                      (int)topGreenNumericUpDown.Value,
                                      (int)topBlueNumericUpDown.Value);
      colorLabel.Text = $"Selected Color Range: {startColor.Name} - {endColor.Name}";
      colorLabel.ForeColor = startColor;
      colorLabel.BackColor = endColor;

      Invalidate();
    }
  }
}
