﻿using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace D3D {
  class ColorRangePicker : Form {
    private NumericUpDown _bottomRed;
    private NumericUpDown _bottomGreen;
    private NumericUpDown _bottomBlue;
    private NumericUpDown _topRed;
    private NumericUpDown _topGreen;
    private NumericUpDown _topBlue;



    public ColorRangePicker() {
      _bottomRed = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(50, 50)
      };

      _bottomGreen = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(110, 50)
      };

      _bottomBlue = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 0,
        Width = 50,
        Location = new Point(170, 50)
      };

      _topRed = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(50, 100)
      };

      _topGreen = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(110, 100)
      };

      _topBlue = new NumericUpDown {
        Minimum = 0,
        Maximum = 255,
        Value = 255,
        Width = 50,
        Location = new Point(170, 100)
      };

      _bottomRed.ValueChanged += (sender, e) => {
        _bottomGreen.Value = ((NumericUpDown)sender).Value;
        UpdateColor();
      };

      _bottomGreen.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      _bottomBlue.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      _topRed.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      _topGreen.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      _topBlue.ValueChanged += (sender, e) => {
        UpdateColor();
      };

      Controls.Add(_bottomRed);
      Controls.Add(_bottomGreen);
      Controls.Add(_bottomBlue);
      Controls.Add(_topRed);
      Controls.Add(_topGreen);
      Controls.Add(_topBlue);


      Paint += (sender, e) => {
        Rectangle gradientRect = new Rectangle(50, 200, 200, 50);
        using (LinearGradientBrush brush = new LinearGradientBrush(gradientRect,
                                                                   Color.FromArgb((int)_bottomRed.Value,
                                                                   (int)_bottomGreen.Value,
                                                                   (int)_bottomBlue.Value),
                                                                   Color.FromArgb((int)_topRed.Value,
                                                                   (int)_topGreen.Value,
                                                                   (int)_topBlue.Value),
                                                                   LinearGradientMode.Vertical)) {
          e.Graphics.FillRectangle(brush, gradientRect);
        }
      };

      FormClosing += (sender, e) => {
        float[] bottom = new float[3];
        bottom[0] = (float)_bottomRed.Value;
        bottom[1] = (float)_bottomGreen.Value;
        bottom[2] = (float)_bottomBlue.Value;

        float[] top = new float[3];
        top[0] = (float)_topRed.Value;
        top[1] = (float)_topGreen.Value;
        top[2] = (float)_topBlue.Value;

        ColorSystem.ChangeColors(bottom, top);
      };

    }

    private void UpdateColor() {
      Invalidate();
    }
  }
}
