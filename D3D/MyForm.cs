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
      _form.AllowUserResizing = false;
      _form.SuspendLayout();
      _form.Name = "MyForm";
      _form.ResumeLayout(false);

      _mousePos = new CMousePos();
      _scene = new List<Scene> {Generator.CreateNewGridTestingScene(),
                                Generator.CreateAnotherPipeTestingScene()};
      _movingParams = new CMouseMovingParams(10.0f, 20.0f);

      ////////////////////////////////////////////////////////////////////
      string text = "W - move forward\nA - move left\nS - move backward\nD - move right\n= - move up\n- - move down\nRMB - movings\nWheel-Pressed - rotation";
      AddLabel("help", text, new System.Drawing.Point(25, 25));
      AddLabel("lights-x", "x-coord", new System.Drawing.Point(25, 160));
      AddLabel("lights-y", "y-coord", new System.Drawing.Point(25, 210));
      AddLabel("lights-z", "z-coord", new System.Drawing.Point(25, 260));

      AddTrackbar("lightX", new System.Drawing.Point(100, 150), ChangeLightPosition);
      AddTrackbar("lightY", new System.Drawing.Point(100, 190), ChangeLightPosition);
      AddTrackbar("lightZ", new System.Drawing.Point(100, 230), ChangeLightPosition);
      AddButton(new System.Drawing.Point(25, 300), "light color", ChangeLightColor);

      AddRadioButton("line", "Line", new System.Drawing.Point(25, 400), ChangePipeShowType);
      AddRadioButton("line", "Line", new System.Drawing.Point(25, 430), ChangePipeShowType);

      AddLabel("sliceX", "slice-x", new System.Drawing.Point(25, 460));
      AddLabel("sliceY", "slice-x", new System.Drawing.Point(25, 510));
      AddLabel("sliceZ", "slice-x", new System.Drawing.Point(25, 560));

      AddTrackbar("sliceX", new System.Drawing.Point(100, 450), CliceGridNewly);
      AddTrackbar("sliceY", new System.Drawing.Point(100, 500), CliceGridNewly);
      AddTrackbar("sliceZ", new System.Drawing.Point(100, 550), CliceGridNewly);

      AddCheckBox("Slice", "Slice", new System.Drawing.Point(200, 450), TurnOnOffSliceMode);

      _form.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _form.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _form.MouseUp += new MouseEventHandler(MyFormMouseUp);
      _form.KeyPress += new KeyPressEventHandler(MyFormKeyPress);
    }

    private void AddRadioButton(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var RadioButton = new RadioButton();
      RadioButton.Name = name;
      RadioButton.Text = text;
      RadioButton.Location = position;
      RadioButton.CheckedChanged += handler;
      _form.Controls.Add(RadioButton);
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


    private void AddTrackbar(string name, System.Drawing.Point position, EventHandler handler) {
      var trackbar = new TrackBar();
      trackbar.Name = name;
      trackbar.Minimum = -100;
      trackbar.Maximum = 100;
      trackbar.TickFrequency = 10;
      trackbar.LargeChange = 10;
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
      checkBox.Checked = true;
      checkBox.Text = "Slicing";
      checkBox.Tag = "Slicing";
      checkBox.Location = position;
      _form.Controls.Add(checkBox);
      checkBox.CheckedChanged += handler;
    }

    private void TurnOnOffSliceMode(Object sender, EventArgs e) {
      var control = _form.Controls.Find("sliceX", true);
      var xTrackbar = control[0] as TrackBar;
      control = _form.Controls.Find("sliceY", true);
      var yTrackbar = control[0] as TrackBar;
      control = _form.Controls.Find("sliceZ", true);
      var zTrackbar = control[0] as TrackBar;

      if (((CheckBox)sender).Checked) {
        CliceGridNewly(null, null);
        xTrackbar.Scroll += CliceGridNewly;
        yTrackbar.Scroll += CliceGridNewly;
        zTrackbar.Scroll += CliceGridNewly;
      } else {
        DrawSystem.RestoreAllGrids();
        xTrackbar.Scroll -= CliceGridNewly;
        yTrackbar.Scroll -= CliceGridNewly;
        zTrackbar.Scroll -= CliceGridNewly;
      }
    }

    public void Run() {
      RenderLoop.Run(_form, RenderCallback);
    }

    private void RenderCallback() {
      var renderer = Renderer.GetRenderer();
      renderer.Update();
      CameraSystem.Update();
      TransformSystem.Update();
      LightSystem.Update();
      DrawSystem.Update();
      renderer.Present();
    }

    private void ChangePipeShowType(object sender, EventArgs e) {
      var bttn = sender as RadioButton;
      if (bttn.Checked) {
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
      var control = _form.Controls.Find("sliceX", true);
      var xTrackbar = control[0] as TrackBar;
      control = _form.Controls.Find("sliceY", true);
      var yTrackbar = control[0] as TrackBar;
      control = _form.Controls.Find("sliceZ", true);
      var zTrackbar = control[0] as TrackBar;
      DrawSystem.CliceGrid(xTrackbar.Value, yTrackbar.Value, zTrackbar.Value);
    }
  }
}
