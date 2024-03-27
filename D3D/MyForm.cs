using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;
using System.Drawing;
using System.Linq;
using static System.Windows.Forms.AxHost;

namespace D3D {

  /// <summary>
  /// Custom form for rendering DirectX scenes.
  /// </summary>
  public class MyForm : RenderForm {

    private List<Scene> _scene;
    private CMouseMovingParams _movingParams;
    private bool _isMouseDown = false;
    private bool _isRotationDown = false;

    private RenderForm _form;
    private CScreenSize _size;
    private CMousePos _mousePos;

    /// <summary>
    /// Constructor for the custom form.
    /// </summary>
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

      _scene = new List<Scene> {Generator.CreateGridTestingScene(),
                                Generator.CreatePipeTestingScene()};

      DrawSystem.ChangePipeAppearance(0.2f, 10);

      _movingParams = new CMouseMovingParams(10.0f, 20.0f);


      AddLabel("pipeMod", "Pipe mod", new System.Drawing.Point(20, 10));
      AddRadioButton("line", "Line", new System.Drawing.Point(20, 30), ChangePipeShowType);
      AddRadioButton("pipe", "Pipe", new System.Drawing.Point(20, 50), ChangePipeShowType);

      AddLabel("Properties", "Properties", new System.Drawing.Point(100, 10));
      AddRadioButton("color", "Color", new System.Drawing.Point(100, 30), ChangeProperty);
      AddRadioButton("stability", "Stability", new System.Drawing.Point(100, 50), ChangeProperty);

      AddLabel("pipeParametersLabel", "Pipe parameters", new System.Drawing.Point(190, 10));
      AddLabel("pipeSegmentsLabel", "Segments", new System.Drawing.Point(190, 40));
      AddLabel("pipeRadiusLabel", "Radius", new System.Drawing.Point(190, 80));
      AddTrackbar("pipeSegments", new System.Drawing.Point(240, 40), ChangePipeParameters, 10, 40, 1);
      AddTrackbar("pipeRadius", new System.Drawing.Point(240, 80), ChangePipeParameters, 1, 20, 1);


      AddCheckBox("Slice", "Slice", new System.Drawing.Point(30, 80), TurnOnOffSliceMode);
      AddLabel("sliceXlabel", "X", new System.Drawing.Point(15, 115));
      AddLabel("sliceYlabel", "Y", new System.Drawing.Point(15, 155));
      AddLabel("sliceZlabel", "Z", new System.Drawing.Point(15, 195));

      AddCheckBox("checkX", "", new System.Drawing.Point(30, 115), SetSliceVisibility);
      AddCheckBox("checkY", "", new System.Drawing.Point(30, 155), SetSliceVisibility);
      AddCheckBox("checkZ", "", new System.Drawing.Point(30, 195), SetSliceVisibility);

      AddTrackbar("sliceX", new System.Drawing.Point(50, 115), CliceGridNewly, 0, 20, 1);
      AddTrackbar("sliceY", new System.Drawing.Point(50, 155), CliceGridNewly, 0, 20, 1);
      AddTrackbar("sliceZ", new System.Drawing.Point(50, 195), CliceGridNewly, 0, 20, 1);

      string[] controls = { "checkX", "checkY", "checkZ", "sliceXlabel", "sliceYlabel", "sliceZlabel", "sliceX", "sliceY", "sliceZ" };
      SetVisibility(false, controls);

      _form.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _form.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _form.MouseUp += new MouseEventHandler(MyFormMouseUp);
      _form.KeyPress += new KeyPressEventHandler(MyFormKeyPress);
    }

    // logic
    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void ChangePipeParameters(Object sender, EventArgs e) {
      TrackBar segments = null;
      TrackBar radius = null;

      var controls = _form.Controls.Find("pipeSegments", true);
      if (controls.Length != 0) {
        foreach(var control in controls) {
          if (control is TrackBar) {
            segments = (TrackBar) control;
            break;
          }
        }
      }

      var radiusControls = _form.Controls.Find("pipeRadius", true);
      if (radiusControls.Length != 0) {
        foreach (var control in radiusControls) {
          if (control is TrackBar) {
            radius = (TrackBar)control;
            break;
          }
        }
      }

      if (!(segments is null) && !(radius is null)) {
        DrawSystem.ChangePipeAppearance(radius.Value / 10.0f, segments.Value);
      }
    }

    /// <summary>
    /// Changes the visibility of a control based on its name.
    /// </summary>
    /// <param name="visible">Visibility state.</param>
    /// <param name="name">Name of the control.</param>
    private void SetVisibility(bool visible, string name) {
      var controls = _form.Controls.Find(name, true);
      foreach (var control in controls) {
        control.Visible = visible;
      }
    }

    /// <summary>
    /// Changes the visibility of slice trackbars according to the related checkbox state.
    /// </summary>
    /// <param name="Sender">The checkbox emitting the signal.</param>
    /// <param name="e">Event arguments.</param>
    private void SetSliceVisibility(Object Sender, EventArgs e) {
      var box = Sender as CheckBox;
      if (box.Checked) {
        if (box.Name == "checkX") {
          SetVisibility(true, "sliceX");
        } else if (box.Name == "checkY") {
          SetVisibility(true, "sliceY");
        } else {
          SetVisibility(true, "sliceZ");
        }
      } else {
        if (box.Name == "checkX") {
          SetVisibility(false, "sliceX");
        } else if (box.Name == "checkY") {
          SetVisibility(false, "sliceY");
        } else {
          SetVisibility(false, "sliceZ");
        }
      }
      CliceGridNewly(null, null);
    }

    /// <summary>
    /// Toggles the slice mode on/off.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TurnOnOffSliceMode(Object sender, EventArgs e) {
      string[] controls = { "checkX", "checkY", "checkZ", "sliceXlabel", "sliceYlabel", "sliceZlabel" };
      if (((CheckBox)sender).Checked) {
        SetVisibility(true, controls);
        CliceGridNewly(null, null);
      } else {
        DrawSystem.RestoreAllGrids();
        SetVisibility(false, controls);
      }
    }

    /// <summary>
    /// Starts the form's rendering loop.
    /// </summary>
    public void Run() {
      RenderLoop.Run(_form, RenderCallback);
    }

    /// <summary>
    /// Callback function for rendering the scene.
    /// </summary>
    private void RenderCallback() {
      var renderer = Renderer.GetRenderer(_form.Handle);
      renderer.Update();
      CameraSystem.Update();
      TransformSystem.Update();
      DrawSystem.Update();
      renderer.Present();
    }

    /// <summary>
    /// Changes the type of pipe to be displayed.
    /// </summary>
    private void ChangePipeShowType(object sender, EventArgs e) {
      var bttn = sender as RadioButton;
      string[] controls = { "pipeParametersLabel", "pipeSegmentsLabel", "pipeRadiusLabel", "pipeSegments", "pipeRadius" };
      if (bttn.Name == "pipe" && bttn.Checked) {
        DrawSystem.ChangePipeType(FigureType.Pipe);
        SetVisibility(true, controls);
      } else {
        DrawSystem.ChangePipeType(FigureType.Line);
        SetVisibility(false, controls);
      }
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
      int x = GetSlicingValue("checkX", "sliceX");
      int y = GetSlicingValue("checkY", "sliceY");
      int z = GetSlicingValue("checkZ", "sliceZ");
      DrawSystem.CliceGrid(x, y, z);
    }

    /// <summary>
    /// Gets the value of a slicing control.
    /// </summary>
    /// <param name="controlName">Name of the checkbox control.</param>
    /// <param name="controlValue">Name of the trackbar control.</param>
    /// <returns>The value of the slicing control.</returns>
    int GetSlicingValue(string controlName, string controlValue) {
      int val = -1;
      var control = _form.Controls.Find(controlName, true);
      var box = control[0] as CheckBox;
      if (box.Checked) {
        control = _form.Controls.Find(controlValue, true);
        var xTrackbar = control[0] as TrackBar;
        val = xTrackbar.Value;
      }
      return val;
    }

    private void ChangeProperty(object sender, EventArgs e) {
      var s = sender as RadioButton;
      if (s.Name == "color") {
        DrawSystem.ChangeProperty(CGridMesh.PropertyType.Color);
      } else if (s.Name == "stability") {
        DrawSystem.ChangeProperty(CGridMesh.PropertyType.Stability);
      }
    }

    // forming
    // ////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Adds a trackbar control to the form.
    /// </summary>
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

    /// <summary>
    /// Adds a button control to the form.
    /// </summary>
    private void AddButton(System.Drawing.Point position, string text, EventHandler handler) {
      var button = new Button();
      button.Location = position;
      button.AutoSize = true;
      button.Text = text;
      _form.Controls.Add(button);
      button.Click += handler;
    }

    /// <summary>
    /// Adds a checkbox control to the form.
    /// </summary>
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

    /// <summary>
    /// Adds a radio button control to the form.
    /// </summary>
    private void AddRadioButton(string name, string text, System.Drawing.Point position, EventHandler handler) {
      var radioButton = new RadioButton();
      radioButton.Name = name;
      radioButton.Text = text;
      radioButton.Location = position;
      radioButton.AutoSize = true;
      radioButton.CheckedChanged += handler;
      _form.Controls.Add(radioButton);
    }

    /// <summary>
    /// Adds a label control to the form.
    /// </summary>
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

    /// <summary>
    /// Sets the visibility state of slicing controls.
    /// </summary>
    private void SetVisibility(bool state, string[] names) {
      foreach (string name in names) {
        SetVisibility(state, name);
      }
    }

  }
}

