using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Windows;
using System.Drawing;
using SharpDX.DirectInput;
using D3D.Engine.System;

namespace D3D {

  /// <summary>
  /// Custom form for rendering DirectX scenes.
  /// </summary>
  public class MyForm : RenderForm {

    private List<Scene> _scene;
    private CMouseMovingParams _movingParams;
    private bool _isRotationDown = false;

    private RenderForm _form;
    private CScreenSize _size;
    private CMousePos _mousePos;
    private KeyboardSystem _keyboard;

    /// <summary>
    /// Constructor for the custom form.
    /// </summary>
    public MyForm(CMouseMovingParams movingdivs) {
      _size = new CScreenSize();
      _form = new RenderForm();
      _form.ClientSize = new Size(_size.Width, _size.Height);
      _form.KeyPreview = true;
      _form.AllowUserResizing = true;
      _form.SuspendLayout();
      _form.Name = "MyForm";
      _form.ResumeLayout(false);
      _mousePos = new CMousePos();
      _keyboard = new KeyboardSystem();
      _scene = new List<Scene>();
      _movingParams = movingdivs;

      _form.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _form.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _form.MouseWheel += new MouseEventHandler(ChangeScale);
      _form.MouseUp += new MouseEventHandler(MyFormMouseUp);

      MenuStrip menuStrip = new MenuStrip();
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Help");
      toolStripMenuItem.Click += ShowHelpMenu;
      menuStrip.Items.Add(toolStripMenuItem);
      _form.Controls.Add(menuStrip);
    }


    public void AddScene(Scene scene) {
      _scene.Add(scene);
    }


    public void AddPipeScene(Scene scene) {
      _scene.Add(scene);
      AddLabel("pipeMod", "Pipe mod", new System.Drawing.Point(20, 30));
      AddRadioButton("line", "Line", new System.Drawing.Point(20, 50), ChangePipeShowType);
      AddRadioButton("pipe", "Pipe", new System.Drawing.Point(20, 70), ChangePipeShowType);

      AddLabel("pipeParametersLabel", "Pipe parameters", new System.Drawing.Point(190, 30));
      AddLabel("pipeSegmentsLabel", "Segments", new System.Drawing.Point(190, 60));
      AddLabel("pipeRadiusLabel", "Radius", new System.Drawing.Point(190, 100));

      AddTrackbar("pipeSegments", new System.Drawing.Point(240, 60), ChangePipeParameters, 10, 40, 1);
      AddTrackbar("pipeRadius", new System.Drawing.Point(240, 100), ChangePipeParameters, 1, 20, 1);
    }


    public void AddGridScene(Scene scene, int[] gridSize) {
      _scene.Add(scene);

      AddLabel("Properties", "Properties", new System.Drawing.Point(100, 30));
      AddComboBox(new System.Drawing.Point(100, 50), new object[] { "color", "stability" });

      AddCheckBox("Slice", "Slice", new System.Drawing.Point(30, 100), TurnOnOffSliceMode);
      AddLabel("sliceXlabel", "X", new System.Drawing.Point(15, 135));
      AddLabel("sliceYlabel", "Y", new System.Drawing.Point(15, 175));
      AddLabel("sliceZlabel", "Z", new System.Drawing.Point(15, 205));

      AddCheckBox("checkX", "", new System.Drawing.Point(30, 135), SetSliceVisibility);
      AddCheckBox("checkY", "", new System.Drawing.Point(30, 175), SetSliceVisibility);
      AddCheckBox("checkZ", "", new System.Drawing.Point(30, 215), SetSliceVisibility);

      AddTrackbar("sliceX", new System.Drawing.Point(50, 135), CliceGridNewly, 0, gridSize[0] - 1, 1); // to be refactored
      var xBar = _form.Controls.Find("sliceX", true);
      TrackBar track = (TrackBar)xBar[0];
      track.Width = 150;

      AddTrackbar("sliceY", new System.Drawing.Point(50, 175), CliceGridNewly, 0, gridSize[1] - 1, 1); // to be refactored
      var yBar = _form.Controls.Find("sliceY", true);
      TrackBar trackY = (TrackBar)yBar[0];
      trackY.Width = 150;

      AddTrackbar("sliceZ", new System.Drawing.Point(50, 215), CliceGridNewly, 0, gridSize[2] - 1, 1); // to be refactored
      var zBar = _form.Controls.Find("sliceZ", true);
      TrackBar trackZ = (TrackBar)zBar[0];
      trackZ.Width = 150;

      AddLabel("sliceXindLabel", "0", new System.Drawing.Point(200, 135));
      AddLabel("sliceYindLabel", "0", new System.Drawing.Point(200, 175));
      AddLabel("sliceZindLabel", "0", new System.Drawing.Point(200, 215));

      AddCheckBox("linesVisibility", "Line Grid", new System.Drawing.Point(300, 30), ChangeLineGridVisibility);

      string[] controls = { "checkX", "checkY", "checkZ",
                            "sliceXlabel", "sliceYlabel", "sliceZlabel",
                            "sliceX", "sliceY", "sliceZ",
                            "sliceXindLabel", "sliceYindLabel", "sliceZindLabel" };
      SetVisibility(false, controls);
    }



    // logic
    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Changes the visibility of the line grid based on the checkbox state.
    /// </summary>
    /// <param name="sender">The object that triggered the event (checkbox).</param>
    /// <param name="e">The event arguments.</param>
    private void ChangeLineGridVisibility(Object sender, EventArgs e) {
      DrawSystem.ChangeLineGridVisibility(((CheckBox)sender).Checked);
    }

    /// <summary>
    /// Displays a help menu with instructions for controls.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ShowHelpMenu(object sender, EventArgs e) {
      Form textWindow = new Form();
      Label label = new Label();
      label.AutoSize = true;
      label.Text = "For moving press W A S D\n" +
        "You can rotate camera by pressing Scroll Mouse Button\n" +
        "You can climb and descend by pressing 'Q' and 'E'\n" +
        "You can adjust scale by mouse scrolling.";
      label.Dock = DockStyle.Fill;
      textWindow.Controls.Add(label);
      textWindow.Show();
    }

    /// <summary>
    /// Changes the scale of the camera based on mouse wheel movement.
    /// </summary>
    /// <param name="sender">The object that triggered the event (camera scale change).</param>
    /// <param name="e">The mouse event arguments.</param>
    private void ChangeScale(object sender, MouseEventArgs e) {
      int factor = 7; // example
      if (e.Delta > 0) {
        for (int i = 0; i < factor; ++i) {
          CameraSystem.ShiftFwd();
        }
      } else {
        for (int i = 0; i < factor; ++i) {
          CameraSystem.ShiftBack();
        }
      }
    }

    /// <summary>
    /// Changes the parameters of a pipe based on the trackbars for segments and radius.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ChangePipeParameters(Object sender, EventArgs e) {
      TrackBar segments = null;
      TrackBar radius = null;

      var controls = _form.Controls.Find("pipeSegments", true);
      if (controls.Length != 0) {
        foreach (var control in controls) {
          if (control is TrackBar) {
            segments = (TrackBar)control;
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
          SetVisibility(true, new string[] { "sliceX", "sliceXindLabel" });
        } else if (box.Name == "checkY") {
          SetVisibility(true, new string[] { "sliceY", "sliceYindLabel" });
        } else {
          SetVisibility(true, new string[] { "sliceZ", "sliceZindLabel" });
        }
      } else {
        if (box.Name == "checkX") {
          SetVisibility(false, new string[] { "sliceX", "sliceXindLabel" });
        } else if (box.Name == "checkY") {
          SetVisibility(false, new string[] { "sliceY", "sliceYindLabel" });
        } else {
          SetVisibility(false, new string[] { "sliceZ", "sliceZindLabel" });
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
      _keyboard.ProcessInput();
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
      } else if (bttn.Name == "line" && bttn.Checked) {
        DrawSystem.ChangePipeType(FigureType.Line);
        SetVisibility(false, controls);
      }
    }

    /// <summary>
    /// Handles the mouse move event to calculate the delta values and apply rotation to the scenes.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The mouse event arguments.</param>
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
        }
      }
    }

    /// <summary>
    /// Handles the mouse down event to set the initial mouse position and start rotation if middle button is pressed.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The mouse event arguments.</param>
    private void MyFormMouseDown(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = e;
      _mousePos.X = mouseArgs.X;
      _mousePos.Y = mouseArgs.Y;

      if (mouseArgs.Button == MouseButtons.Middle) {
        _isRotationDown = true;
      }
    }

    /// <summary>
    /// Handles the mouse up event to stop rotation when the middle button is released.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The mouse event arguments.</param>
    private void MyFormMouseUp(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Middle) {
        _isRotationDown = false;
      }
    }

    /// <summary>
    /// Gets values of slicing controls and sets theese values to apporpriate objects.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CliceGridNewly(object sender, EventArgs e) {
      int x = GetSlicingValue("checkX", "sliceX");
      int y = GetSlicingValue("checkY", "sliceY");
      int z = GetSlicingValue("checkZ", "sliceZ");

      SetSlicingValue("sliceXindLabel", x);
      SetSlicingValue("sliceYindLabel", y);
      SetSlicingValue("sliceZindLabel", z);

      DrawSystem.CliceGrid(x, y, z);
    }

    /// <summary>
    /// Sets the control value to the text of control (CONTROL SHOULD BE LABEL)
    /// </summary>
    /// <param name="controlName">name of label</param>
    /// <param name="val">value to be set</param>
    private void SetSlicingValue(string controlName, int val) {
      var control = _form.Controls.Find(controlName, true);
      var lbl = control[0] as Label;
      lbl.Text = val.ToString();
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

    /// <summary>
    /// Changes property of registered GridMeshes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeComboProperty(object sender, EventArgs e) {
      ComboBox comboBox = (ComboBox)sender;
      string selectedOption = comboBox.SelectedItem.ToString();

      if (selectedOption == "color") {
        DrawSystem.ChangeProperty(CGridMesh.PropertyType.Color);
      } else if (selectedOption == "stability") {
        DrawSystem.ChangeProperty(CGridMesh.PropertyType.Stability);
      }
    }

    // forming
    // ////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Adds a trackbar control to the form.
    /// </summary>
    private void AddTrackbar(string name, System.Drawing.Point position,
                             EventHandler handler, int bottom, int top, int step) {
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
    /// Adds a ComboBox control to the form.
    /// </summary>
    private void AddComboBox(System.Drawing.Point pos, object[] obj) {
      ComboBox comboBox = new ComboBox();
      comboBox.DropDownWidth = 65;
      comboBox.Items.AddRange(obj);
      comboBox.Location = pos;
      comboBox.Size = new System.Drawing.Size(65, 21);
      _form.Controls.Add(comboBox);
      comboBox.SelectedIndexChanged += ChangeComboProperty;
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

