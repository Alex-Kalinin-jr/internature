using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;
using System.Drawing;


namespace D3D {

  public class MyForm : RenderForm {

    private List<Scene> _scene;
    private CMouseMovingParams _movingParams;
    private bool _isMouseDown = false;
    private bool _isRotationDown = false;

    private RenderForm _form;
    private CScreenSize _size;
    private CMousePos _mousePos;

    public MyForm() {
      _size = new CScreenSize();

      _form = new RenderForm();
      _form.ClientSize = new Size(_size.Width, _size.Height);
      _form.KeyPreview = true;
      _form.AllowUserResizing = true;
      _form.SuspendLayout();
      _form.Name = "MyForm";
      _form.ResumeLayout(false);

      _mousePos = new CMousePos();
      _scene = new List<Scene> {Generator.CreateNewGridTestingScene(),
                                Generator.CreateAnotherPipeTestingScene()};
      _movingParams = new CMouseMovingParams(10.0f, 20.0f);


      AddLabel("pipeMod", "Pipe mod", new System.Drawing.Point(20, 10));
      AddRadioButton("line", "Line", new System.Drawing.Point(20, 30), ChangePipeShowType);
      AddRadioButton("pipe", "Pipe", new System.Drawing.Point(20, 50), ChangePipeShowType);


      AddCheckBox("Slice", "Slice", new System.Drawing.Point(30, 80), TurnOnOffSliceMode);
      AddLabel("sliceXlabel", "X", new System.Drawing.Point(15, 115));
      AddLabel("sliceYlabel", "Y", new System.Drawing.Point(15, 155));
      AddLabel("sliceZlabel", "Z", new System.Drawing.Point(15, 195));

      AddCheckBox("checkX", "", new System.Drawing.Point(30, 115), SetXSliceVisibility);
      AddCheckBox("checkY", "", new System.Drawing.Point(30, 155), SetYSliceVisibility);
      AddCheckBox("checkZ", "", new System.Drawing.Point(30, 195), SetZSliceVisibility);

      AddTrackbar("sliceX", new System.Drawing.Point(50, 115), CliceGridNewly, 0, 20, 1);
      AddTrackbar("sliceY", new System.Drawing.Point(50, 155), CliceGridNewly, 0, 20, 1);
      AddTrackbar("sliceZ", new System.Drawing.Point(50, 195), CliceGridNewly, 0, 20, 1);

      SetSlicingVisibility(false);
      SetVisibility(false, "sliceX");
      SetVisibility(false, "sliceY");
      SetVisibility(false, "sliceZ");

      _form.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _form.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _form.MouseUp += new MouseEventHandler(MyFormMouseUp);
      _form.KeyPress += new KeyPressEventHandler(MyFormKeyPress);
    }
    // logic
    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////


    private void SetVisibility(bool visible, string name) {
      var controls = _form.Controls.Find(name, true);
      foreach (var control in controls) {
        control.Visible = visible;
      }
    }

    private void SetXSliceVisibility(Object Sender, EventArgs e) {
      var box = Sender as CheckBox;
      if (box.Checked) {
        SetVisibility(true, "sliceX");
      } else {
        SetVisibility(false, "sliceX");
      }
      CliceGridNewly(null, null);
    }

    private void SetYSliceVisibility(Object Sender, EventArgs e) {
      var box = Sender as CheckBox;
      if (box.Checked) {
        SetVisibility(true, "sliceY");
      } else {
        SetVisibility(false, "sliceY");
      }
      CliceGridNewly(null, null);
    }

    private void SetZSliceVisibility(Object Sender, EventArgs e) {
      var box = Sender as CheckBox;
      if (box.Checked) {
        SetVisibility(true, "sliceZ");
      } else {
        SetVisibility(false, "sliceZ");
      }
      CliceGridNewly(null, null);
    }

    private void TurnOnOffSliceMode(Object sender, EventArgs e) {
      if (((CheckBox)sender).Checked) {
        SetSlicingVisibility(true);
        CliceGridNewly(null, null);
      } else {
        DrawSystem.RestoreAllGrids();
        SetSlicingVisibility(false);
      }
    }

    public void Run() {
      RenderLoop.Run(_form, RenderCallback);
    }

    private void RenderCallback() {
      var renderer = Renderer.GetRenderer(_form.Handle);
      renderer.Update();
      CameraSystem.Update();
      TransformSystem.Update();
      LightSystem.Update();
      DrawSystem.Update();
      renderer.Present();
    }

    private void ChangePipeShowType(object sender, EventArgs e) {
      var bttn = sender as RadioButton;
      if (bttn.Name == "pipe" && bttn.Checked) {
        DrawSystem.ChangePipeType(FigureType.Pipe);
      } else {
        DrawSystem.ChangePipeType(FigureType.Line);
      }
    }

    private void ChangeLightColor(object sender, EventArgs e) {
      ColorDialog buff = new ColorDialog();
      buff.AllowFullOpen = false;
      buff.ShowHelp = true;

      if (buff.ShowDialog() == DialogResult.OK) {
        var color = new Vector4(buff.Color.R / 255, buff.Color.G / 255, buff.Color.B / 255, buff.Color.A / 255);
        LightSystem.ChangeColor(color);
      }
    }

    private void ChangeLightPosition(object sender, EventArgs e) {
      var control = _form.Controls.Find("lightX", true);
      var xTracbar = control[0] as TrackBar;
      control = _form.Controls.Find("lightY", true);
      var yTracbar = control[0] as TrackBar;
      control = _form.Controls.Find("lightZ", true);
      var zTracbar = control[0] as TrackBar;

      float xDirection = xTracbar.Value / _movingParams.ShiftDivider;
      float yDirection = yTracbar.Value / _movingParams.ShiftDivider;
      float zDirection = zTracbar.Value / _movingParams.ShiftDivider;

      LightSystem.ChangePosition(new Vector3(xDirection, yDirection, zDirection));
    }

    private void MyFormKeyPress(object sender, KeyPressEventArgs e) {
      foreach (var scene in _scene) {
        if (e.KeyChar == 'w') {
          CameraSystem.ShiftFwd();
        } else if (e.KeyChar == 's') {
          CameraSystem.ShiftBack();
        } else if (e.KeyChar == 'd') {
          CameraSystem.ShiftRight();
        } else if (e.KeyChar == 'a') {
          CameraSystem.ShiftLeft();
        } else if (e.KeyChar == '=') {
          CameraSystem.ShiftUp();
        } else if (e.KeyChar == '-') {
          CameraSystem.ShiftDown();
        }
      }
    }

    private void MyFormMouseMove(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = e;
      var deltaX = mouseArgs.X - _mousePos.X;
      var deltaY = mouseArgs.Y - _mousePos.Y;
      _mousePos.X = mouseArgs.X;
      _mousePos.Y = mouseArgs.Y;

      foreach (var scene in _scene) {
        if (_isRotationDown) {
          scene.AddComponent(new CPitch(deltaY / _movingParams.RotDivider));
          scene.AddComponent(new CYaw(deltaX / _movingParams.RotDivider));
        } else if (_isMouseDown) {

          if (deltaX > 0) {
            CameraSystem.ShiftLeft();
          } else {
            CameraSystem.ShiftRight();
          }

          if (deltaY > 0) {
            CameraSystem.ShiftUp();
          } else {
            CameraSystem.ShiftDown();
          }
        }
      }
    }

    private void MyFormMouseDown(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = e;
      _mousePos.X = mouseArgs.X;
      _mousePos.Y = mouseArgs.Y;

      if (mouseArgs.Button == MouseButtons.Left) {
        _isMouseDown = true;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        _isRotationDown = true;
      }
    }

    private void MyFormMouseUp(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      if (mouseArgs.Button == MouseButtons.Left) {
        _isMouseDown = false;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        _isRotationDown = false;
      }
    }

    private void CliceGridNewly(object sender, EventArgs e) {
      int x = -1;
      int y = -1;
      int z = -1;

      var control = _form.Controls.Find("checkX", true);
      var box = control[0] as CheckBox;
      if (box.Checked) {
        control = _form.Controls.Find("sliceX", true);
        var xTrackbar = control[0] as TrackBar;
        x = xTrackbar.Value;
      }

      control = _form.Controls.Find("checkY", true);
      box = control[0] as CheckBox;
      if (box.Checked) {
        control = _form.Controls.Find("sliceY", true);
        var xTrackbar = control[0] as TrackBar;
        y = xTrackbar.Value;
      }

      control = _form.Controls.Find("checkZ", true);
      box = control[0] as CheckBox;
      if (box.Checked) {
        control = _form.Controls.Find("sliceZ", true);
        var xTrackbar = control[0] as TrackBar;
        z = xTrackbar.Value;
      }
      DrawSystem.CliceGrid(x, y, z);
    }

    // forming
    // ////////////////////////////////////////////////////////////////////////////////////
    private void AddTrackbar(string name, System.Drawing.Point position, EventHandler handler, int bottom, int top, int step) {
      var trackbar = new TrackBar();
      trackbar.Name = name;
      trackbar.Minimum = bottom;
      trackbar.Maximum = top;
      trackbar.TickFrequency = step;
      trackbar.LargeChange = step * 3;
      trackbar.Location = position;
      trackbar.Scroll += handler;
      _form.Controls.Add(trackbar);
    }

    private void AddButton(System.Drawing.Point position, string text, EventHandler handler) {
      var button = new Button();
      button.Location = position;
      button.AutoSize = true;
      button.Text = text;
      _form.Controls.Add(button);
      button.Click += handler;
    }

    private void AddCheckBox(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var checkBox = new CheckBox();
      checkBox.Checked = false;
      checkBox.AutoSize = true;
      checkBox.Text = text;
      checkBox.Name = name;
      checkBox.Location = position;
      checkBox.CheckedChanged += handler;
      _form.Controls.Add(checkBox);
    }

    private void AddRadioButton(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var radioButton = new RadioButton();
      radioButton.Name = name;
      radioButton.Text = text;
      radioButton.Location = position;
      radioButton.AutoSize = true;
      radioButton.CheckedChanged += handler;
      _form.Controls.Add(radioButton);
    }

    private void AddLabel(string name, string text, System.Drawing.Point pos) {
      Label label = new Label();
      label.AutoSize = true;
      label.Location = pos;
      label.Name = name;
      label.TabIndex = 0;
      label.Text = text;
      label.TextAlign = ContentAlignment.MiddleLeft;
      _form.Controls.Add(label);
    }

    private void SetSlicingVisibility(bool state) {
      SetVisibility(state, "checkX");
      SetVisibility(state, "checkY");
      SetVisibility(state, "checkZ");
      SetVisibility(state, "sliceXlabel");
      SetVisibility(state, "sliceYlabel");
      SetVisibility(state, "sliceZlabel");
    }

  }
}
