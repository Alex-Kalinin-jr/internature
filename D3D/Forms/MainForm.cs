using SharpDX.Windows;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class MainForm : RenderForm {

    protected MainForm() { }

    protected TrackBar AddTrackbar(string name, System.Drawing.Point position,
                             EventHandler handler, int bottom, int top, int step) {
      var trackbar = new TrackBar();
      trackbar.Name = name;
      trackbar.Minimum = bottom;
      trackbar.Maximum = top;
      trackbar.TickFrequency = step;
      trackbar.LargeChange = step * 3;
      trackbar.Location = position;
      trackbar.Scroll += handler;
      return trackbar;
    }

    protected Button AddButton(System.Drawing.Point position, string text, EventHandler handler) {
      var button = new Button();
      button.Location = position;
      button.AutoSize = true;
      button.Text = text;
      button.Click += handler;
      return button;
    }

    protected CheckBox AddCheckBox(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var checkBox = new CheckBox();
      checkBox.Checked = false;
      checkBox.AutoSize = true;
      checkBox.Text = text;
      checkBox.Name = name;
      checkBox.Location = position;
      checkBox.CheckedChanged += handler;
      return checkBox;
    }

    protected RadioButton AddRadioButton(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var radioButton = new RadioButton();
      radioButton.Name = name;
      radioButton.Text = text;
      radioButton.Location = position;
      radioButton.AutoSize = true;
      radioButton.CheckedChanged += handler;
      return radioButton;
    }

    protected Label AddLabel(string name, string text, System.Drawing.Point pos) {
      Label label = new Label();
      label.AutoSize = true;
      label.Location = pos;
      label.Name = name;
      label.TabIndex = 0;
      label.Text = text;
      label.TextAlign = ContentAlignment.MiddleLeft;
      return label;
    }

    protected ComboBox AddComboBox(string name, System.Drawing.Point pos, object[] obj) {
      ComboBox comboBox = new ComboBox();
      comboBox.Name = name;
      comboBox.Width = 80;
      comboBox.DropDownWidth = 80;
      comboBox.Items.AddRange(obj);
      comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBox.Location = pos;
      comboBox.Size = new System.Drawing.Size(65, 21);
      // comboBox.SelectedIndexChanged += ChangeComboProperty; // DO NOT FORGET TO DO THIS
      return comboBox;
    }

    protected void SetVisibility(bool state, string[] names) {
      foreach (string name in names) {
        SetVisibility(state, name);
      }
    }

    protected void SetVisibility(bool visible, string name) {
      var controls = Controls.Find(name, true);
      foreach (var control in controls) {
        control.Visible = visible;
      }
    }

  }
}
