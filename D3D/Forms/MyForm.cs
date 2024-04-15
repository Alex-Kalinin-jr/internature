using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Windows;
using System.Drawing;

namespace D3D {

  /// <summary>
  /// Custom form for rendering DirectX scenes.
  /// </summary>
  public class MyForm : MainForm {
    private WhiteRectangle _control;
    private List<Scene> _scene;
    private CMouseMovingParams _movingParams;
    private RenderForm _form;
    private CScreenSize _size;
    private CMousePos _mousePos;
    private KeyboardSystem _keyboard;
    private ColorRangePicker _colorRangePicker;
    private PipeForm _pipeChanger;
    private bool _isRotationDown = false;

    private Label _propLabel;
    private ComboBox _propComboBox;
    private CheckBox _sliceheckBox;
    private Label _sliceXlabel;
    private Label _sliceYlabel;
    private Label _sliceZlabel;
    private CheckBox _xCheckBox;
    private CheckBox _yCheckBox;
    private CheckBox _zCheckBox;
    private TrackBar _xTrackBar;
    private TrackBar _yTrackBar;
    private TrackBar _zTrackBar;
    private Label _xLabel;
    private Label _yLabel;
    private Label _zLabel;
    private CheckBox _lineBox;

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
      _form.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _form.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _form.MouseWheel += new MouseEventHandler(ChangeScale);
      _form.MouseUp += new MouseEventHandler(MyFormMouseUp);

      _mousePos = new CMousePos();
      _keyboard = new KeyboardSystem();
      _scene = new List<Scene>();
      _movingParams = movingdivs;
      _colorRangePicker = new ColorRangePicker();
      _pipeChanger = new PipeForm();

      MenuStrip menuStrip = new MenuStrip();
      _form.Controls.Add(menuStrip);
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Help");
      toolStripMenuItem.Click += ShowHelpMenu;
      menuStrip.Items.Add(toolStripMenuItem);
      ToolStripMenuItem colorPicker = new ToolStripMenuItem("Color");
      colorPicker.Click += ShowColorMenu;
      menuStrip.Items.Add(colorPicker);
      ToolStripMenuItem pipeChanger = new ToolStripMenuItem("Pipe");
      pipeChanger.Click += ShowPipeMenu;
      menuStrip.Items.Add(pipeChanger);
    }

    public void AddScene(Scene scene) {
      _scene.Add(scene);
    }

    public void AddPipeScene(Scene scene) {
      _scene.Add(scene);
    }

    public void AddGridScene(Scene scene, int[] gridSize) {
      _scene.Add(scene);

      _control = new WhiteRectangle();
      _control.Width = 250;
      _control.Height = 2550;
      _control.BackColor = Color.White;
      _control.Location = new Point(0, 20);
      _form.Controls.Add(_control);
      _form.Controls.SetChildIndex(_control, 0);

      _lineBox = AddCheckBox("linesVisibility", "Line Grid", new System.Drawing.Point(70, 50), ChangeLineGridVisibility);
      _form.Controls.Add(_lineBox);
      _form.Controls.SetChildIndex(_lineBox, 0);

      _sliceheckBox = AddCheckBox("Slice", "Slice", new System.Drawing.Point(20, 50), TurnOnOffSliceMode);
      _form.Controls.Add(_sliceheckBox);
      _form.Controls.SetChildIndex(_sliceheckBox, 0);

      _sliceXlabel = AddLabel("sliceXlabel", "X", new System.Drawing.Point(15, 70));
      _form.Controls.Add(_sliceXlabel);
      _form.Controls.SetChildIndex(_sliceXlabel, 1);

      _sliceYlabel = AddLabel("sliceYlabel", "Y", new System.Drawing.Point(15, 110));
      _form.Controls.Add(_sliceYlabel);
      _form.Controls.SetChildIndex(_sliceYlabel, 1);

      _sliceZlabel = AddLabel("sliceZlabel", "Z", new System.Drawing.Point(15, 150));
      _form.Controls.Add(_sliceZlabel);
      _form.Controls.SetChildIndex(_sliceZlabel, 1);

      _xCheckBox = AddCheckBox("checkX", "", new System.Drawing.Point(30, 70), SetSliceVisibility);
      _form.Controls.Add(_xCheckBox);
      _form.Controls.SetChildIndex(_xCheckBox, 1);

      _yCheckBox = AddCheckBox("checkY", "", new System.Drawing.Point(30, 110), SetSliceVisibility);
      _form.Controls.Add(_yCheckBox);
      _form.Controls.SetChildIndex(_yCheckBox, 1);

      _zCheckBox = AddCheckBox("checkZ", "", new System.Drawing.Point(30, 150), SetSliceVisibility);
      _form.Controls.Add(_zCheckBox);
      _form.Controls.SetChildIndex(_zCheckBox, 1);

      _xTrackBar = AddTrackbar("sliceX", new System.Drawing.Point(50, 70), CliceGridNewly, 0, gridSize[0] - 1, 1); // to be refactored
      _xTrackBar.Width = 150;
      _form.Controls.Add(_xTrackBar);
      _form.Controls.SetChildIndex(_xTrackBar, 1);

      _yTrackBar = AddTrackbar("sliceY", new System.Drawing.Point(50, 110), CliceGridNewly, 0, gridSize[1] - 1, 1); // to be refactored
      _yTrackBar.Width = 150;
      _form.Controls.Add(_yTrackBar);
      _form.Controls.SetChildIndex(_yTrackBar, 1);

      _zTrackBar = AddTrackbar("sliceZ", new System.Drawing.Point(50, 150), CliceGridNewly, 0, gridSize[2] - 1, 1); // to be refactored
      _zTrackBar.Width = 150;
      _form.Controls.Add(_zTrackBar);
      _form.Controls.SetChildIndex(_zTrackBar, 1);

      _xLabel = AddLabel("sliceXindLabel", "0", new System.Drawing.Point(200, 70));
      _form.Controls.Add(_xLabel);
      _form.Controls.SetChildIndex(_xLabel, 1);

      _yLabel = AddLabel("sliceYindLabel", "0", new System.Drawing.Point(200, 110));
      _form.Controls.Add(_yLabel);
      _form.Controls.SetChildIndex(_yLabel, 1);

      _zLabel = AddLabel("sliceZindLabel", "0", new System.Drawing.Point(200, 150));
      _form.Controls.Add(_zLabel);
      _form.Controls.SetChildIndex(_zLabel, 1);

      _propLabel = AddLabel("Properties", "Properties", new System.Drawing.Point(140, 30));
      _form.Controls.Add(_propLabel);
      _form.Controls.SetChildIndex(_propLabel, 0);

      _propComboBox = AddComboBox("properties", new System.Drawing.Point(140, 50), DrawSystem.GetAllGridsProperties().ToArray());
      _form.Controls.Add(_propComboBox);
      _form.Controls.SetChildIndex(_propComboBox, 1);
      _propComboBox.SelectedIndexChanged += ChangeComboProperty;

      string[] controls = { "checkX", "checkY", "checkZ",
                            "sliceXlabel", "sliceYlabel", "sliceZlabel",
                            "sliceX", "sliceY", "sliceZ",
                            "sliceXindLabel", "sliceYindLabel", "sliceZindLabel" };
      SetVisibility(false, controls);
    }

    /// <summary>
    /// Starts the form's rendering loop.
    /// </summary>
    public void Run() {
      RenderLoop.Run(_form, RenderCallback);
    }

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

    private void ShowColorMenu(object sender, EventArgs e) {
      _colorRangePicker.Show();
    }

    private void ShowPipeMenu(object sender, EventArgs e) {
      _pipeChanger.Show();
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
      string[] controls = { "checkX", "checkY", "checkZ", "sliceXlabel", "sliceYlabel", "sliceZlabel"};
      if (((CheckBox)sender).Checked) {
        SetVisibility(true, controls);
        CliceGridNewly(null, null);
      } else {
        DrawSystem.RestoreAllGrids();
        string[] additionalControls = { "sliceX", "sliceY", "sliceZ", "sliceXindLabel", "sliceYindLabel", "sliceZindLabel" };
        SetVisibility(false, controls);
        SetVisibility(false, additionalControls);
      }
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
      ColorSystem.ChangeProperty(comboBox.SelectedItem.ToString());
    }

    private new void SetVisibility(bool state, string[] names) {
      foreach (string name in names) {
        SetVisibility(state, name);
      }
    }

    private new void SetVisibility(bool visible, string name) {
      var controls = _form.Controls.Find(name, true);
      foreach (var control in controls) {
        control.Visible = visible;
      }
    }
  }
}

