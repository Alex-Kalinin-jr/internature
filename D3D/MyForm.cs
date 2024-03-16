using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;

namespace D3D {

  public class MyForm : RenderForm {

    new private const int Width = 1024;
    new private const int Height = 768;

    private MousePos _mouse;

    private RenderForm _renderForm;
    private Renderer _renderer;
    private DataStorage _dataStorage;

    private TrackBar _xDirectionTrackBar;
    private TrackBar _yDirectionTrackBar;
    private TrackBar _zDirectionTrackBar;


    bool _isMouseDown = false;
    bool _isRotationDown = false;

    public MyForm() {
      CreateRenderForm();
      CreateHelpLabel();
      CreateDiffuseColor();
      CreateDirectionTrackBars();

      _dataStorage = Generator.CreateTestingDataStorage();
      _renderer = Renderer.GetRenderer(_renderForm.Handle);
      _mouse = new MousePos();
    }

    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    private void RenderCallback() {
      _renderer.RenderCallback(_dataStorage);
    }

    private void CreateHelpLabel() {
      Label _labelHelp = new Label();
      _labelHelp.AutoSize = true;
      _labelHelp.Location = new System.Drawing.Point(25, 25);
      _labelHelp.Name = "help";
      _labelHelp.TabIndex = 0;
      _labelHelp.Text = "WASD- movings\n-= - Up-Down movings\nMouseWheelPressed - rotation of object\nRMB - rotation of camera";
      _labelHelp.TextAlign = ContentAlignment.MiddleLeft;
      _renderForm.Controls.Add(_labelHelp);
    }

    private void CreateDiffuseColor() {
      Button _diffuseLightColor = new Button();
      _diffuseLightColor.Location = new System.Drawing.Point(25, 100);
      _diffuseLightColor.Size = new Size(100, 25);
      _diffuseLightColor.Text = "light color";
      _diffuseLightColor.Click += ChangeDiffuseColor;
      _renderForm.Controls.Add(_diffuseLightColor);
    }

    private void ChangeDiffuseColor(object sender, EventArgs e) {

      ColorDialog buff = new ColorDialog();
      buff.AllowFullOpen = false;
      buff.ShowHelp = true;
      if (buff.ShowDialog() == DialogResult.OK) {
        _dataStorage.ChangeLightColor(new Vector4(buff.Color.R / 255, buff.Color.G / 255,
                                                  buff.Color.B / 255, buff.Color.A / 255));
      }
    }

    private void CreateDirectionTrackBars() {

      Label x = new Label();
      x.Text = "x-coord";
      x.Location = new System.Drawing.Point(25, 160);
      _renderForm.Controls.Add(x);

      _xDirectionTrackBar = new TrackBar();
      _xDirectionTrackBar.Minimum = -100;
      _xDirectionTrackBar.Maximum = 100;
      _xDirectionTrackBar.TickFrequency = 10;
      _xDirectionTrackBar.LargeChange = 10;
      _xDirectionTrackBar.Scroll += DirectionTrackBarScroll;
      _xDirectionTrackBar.Location = new System.Drawing.Point(100, 150);

      Label y = new Label();
      y.Text = "y-coord";
      y.Location = new System.Drawing.Point(25, 210);
      _renderForm.Controls.Add(y);

      _yDirectionTrackBar = new TrackBar();
      _yDirectionTrackBar.Minimum = -100;
      _yDirectionTrackBar.Maximum = 100;
      _yDirectionTrackBar.TickFrequency = 10;
      _yDirectionTrackBar.LargeChange = 10;
      _yDirectionTrackBar.Scroll += DirectionTrackBarScroll;
      _yDirectionTrackBar.Location = new System.Drawing.Point(100, 200);

      Label z = new Label();
      z.Text = "z-coord";
      z.Location = new System.Drawing.Point(25, 260);
      _renderForm.Controls.Add(z);

      _zDirectionTrackBar = new TrackBar();
      _zDirectionTrackBar.Minimum = -100;
      _zDirectionTrackBar.Maximum = 100;
      _zDirectionTrackBar.TickFrequency = 10;
      _zDirectionTrackBar.LargeChange = 10;
      _zDirectionTrackBar.Scroll += DirectionTrackBarScroll;
      _zDirectionTrackBar.Location = new System.Drawing.Point(100, 250);

      _renderForm.Controls.Add(_xDirectionTrackBar);
      _renderForm.Controls.Add(_yDirectionTrackBar);
      _renderForm.Controls.Add(_zDirectionTrackBar);
    }

    private void DirectionTrackBarScroll(object sender, EventArgs e) {
      float xDirection = _xDirectionTrackBar.Value / 20.0f;
      float yDirection = _yDirectionTrackBar.Value / 20.0f;
      float zDirection = _zDirectionTrackBar.Value / 20.0f;
      _dataStorage.ChangeLightDirectiron(new Vector3(xDirection, yDirection, zDirection));
    }

    private void CreateRenderForm() {
      _renderForm = new RenderForm();
      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.KeyPreview = true;
      _renderForm.AllowUserResizing = false;
      _renderForm.SuspendLayout();
      _renderForm.Name = "MyForm";
      _renderForm.MouseDown += new MouseEventHandler(this.MyFormMouseDown);
      _renderForm.MouseMove += new MouseEventHandler(this.MyFormMouseMove);
      _renderForm.MouseUp += new MouseEventHandler(this.MyFormMouseUp);
      _renderForm.ResumeLayout(false);
      _renderForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MyFormKeyPress);
    }

    private void MyFormKeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == 'w') {
        _renderer.MoveCameraFwd();
      } else if (e.KeyChar == 's') {
        _renderer.MoveCameraBack();
      } else if (e.KeyChar == 'd') {
        _renderer.MoveCameraRight();
      } else if (e.KeyChar == 'a') {
        _renderer.MoveCameraLeft();
      } else if (e.KeyChar == '=') {
        _renderer.MoveCameraUp();
      } else if (e.KeyChar == '-') {
        _renderer.MoveCameraDown();
      }
    }

    private void MyFormMouseMove(object sender, MouseEventArgs e) {

      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      var deltaX = mouseArgs.X - _mouse.X;
      var deltaY = mouseArgs.Y - _mouse.Y;

      if (_isRotationDown) {
        _renderer.ChangePitch(deltaY / 10.0f);
        _renderer.ChangeYaw(deltaX / 10.0f);
        _mouse.X = mouseArgs.X;
        _mouse.Y = mouseArgs.Y;
      } else if (_isMouseDown) {

        if (deltaX > 0) {
          _renderer.MoveCameraLeft();
        } else {
          _renderer.MoveCameraRight();
        }

        if (deltaY > 0) {
          _renderer.MoveCameraUp();
        } else {
          _renderer.MoveCameraDown();
        }

      }
    }

    private void MyFormMouseDown(object sender, MouseEventArgs e) {

      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      _mouse.X = mouseArgs.X;
      _mouse.Y = mouseArgs.Y;

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

  }
}
