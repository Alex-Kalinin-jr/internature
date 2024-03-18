using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;
using Assimp;

namespace D3D {

  public class MyForm : RenderForm {

    new private const int Width = 1024;
    new private const int Height = 768;

    private MousePos _mouse;
    private List<Scene> _scene;

    private RenderForm _renderForm;

    private TrackBar _xPositionTrackBar;
    private TrackBar _yPositionTrackBar;
    private TrackBar _zPositionTrackBar;

    private bool _isMouseDown = false;
    private bool _isRotationDown = false;

    private const float _rotationSpeed = 10.0f;
    private const float _positionSpeed = 20.0f;

    public MyForm() {
      CreateRenderForm();
      CreateHelpLabel();
      CreateLightColorButton();
      CreateDirectionTrackBars();

      _mouse = new MousePos();
      _scene = new List<Scene>();
      _scene.Add(Generator.CreateGridTestingScene());
      _scene.Add(Generator.CreateTestingScene());

    }

    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    private void RenderCallback() {
      var renderer = Renderer.GetRenderer();
      renderer.Update();
      CameraSystem.Update();
      TransformSystem.Update();
      SLightSystem.Update();
      DrawSystem.Update();
      renderer.Present();
    }

    private void CreateHelpLabel() {
      Label _labelHelp = new Label();
      _labelHelp.AutoSize = true;
      _labelHelp.Location = new System.Drawing.Point(25, 25);
      _labelHelp.Name = "help";
      _labelHelp.TabIndex = 0;
      string text = "W - move forward\nA - move left\nS - move backward\nD - move right\n= - move up\n- - move down\nRMB - movings\nWheel-Pressed - rotation";
      _labelHelp.Text = text;
      _labelHelp.TextAlign = ContentAlignment.MiddleLeft;
      _renderForm.Controls.Add(_labelHelp);
    }

    private void CreateLightColorButton() {
      Button _diffuseLightColor = new Button();
      _diffuseLightColor.Location = new System.Drawing.Point(25, 300);
      _diffuseLightColor.Size = new Size(100, 25);
      _diffuseLightColor.Text = "light color";
      _diffuseLightColor.Click += ChangeLightColor;
      _renderForm.Controls.Add(_diffuseLightColor);
    }

    private void ChangeLightColor(object sender, EventArgs e) {
      ColorDialog buff = new ColorDialog();
      buff.AllowFullOpen = false;
      buff.ShowHelp = true;
      if (buff.ShowDialog() == DialogResult.OK) {
        var color = new Vector4(buff.Color.R / 255, buff.Color.G / 255,
                                                  buff.Color.B / 255, buff.Color.A / 255);
        foreach (var scene in _scene) {
          scene.AddComponent(new CNewLightColor(color));
        }
      }
    }

    private void CreateDirectionTrackBars() {

      Label x = new Label();
      x.Text = "x-coord";
      x.Location = new System.Drawing.Point(25, 160);
      _renderForm.Controls.Add(x);

      _xPositionTrackBar = new TrackBar();
      _xPositionTrackBar.Minimum = -100;
      _xPositionTrackBar.Maximum = 100;
      _xPositionTrackBar.TickFrequency = 10;
      _xPositionTrackBar.LargeChange = 10;
      _xPositionTrackBar.Scroll += DirectionTrackBarScroll;
      _xPositionTrackBar.Location = new System.Drawing.Point(100, 150);

      Label y = new Label();
      y.Text = "y-coord";
      y.Location = new System.Drawing.Point(25, 210);
      _renderForm.Controls.Add(y);

      _yPositionTrackBar = new TrackBar();
      _yPositionTrackBar.Minimum = -100;
      _yPositionTrackBar.Maximum = 100;
      _yPositionTrackBar.TickFrequency = 10;
      _yPositionTrackBar.LargeChange = 10;
      _yPositionTrackBar.Scroll += DirectionTrackBarScroll;
      _yPositionTrackBar.Location = new System.Drawing.Point(100, 200);

      Label z = new Label();
      z.Text = "z-coord";
      z.Location = new System.Drawing.Point(25, 260);
      _renderForm.Controls.Add(z);

      _zPositionTrackBar = new TrackBar();
      _zPositionTrackBar.Minimum = -100;
      _zPositionTrackBar.Maximum = 100;
      _zPositionTrackBar.TickFrequency = 10;
      _zPositionTrackBar.LargeChange = 10;
      _zPositionTrackBar.Scroll += DirectionTrackBarScroll;
      _zPositionTrackBar.Location = new System.Drawing.Point(100, 250);

      _renderForm.Controls.Add(_xPositionTrackBar);
      _renderForm.Controls.Add(_yPositionTrackBar);
      _renderForm.Controls.Add(_zPositionTrackBar);
    }

    private void DirectionTrackBarScroll(object sender, EventArgs e) {
      float xDirection = _xPositionTrackBar.Value / _positionSpeed;
      float yDirection = _yPositionTrackBar.Value / _positionSpeed;
      float zDirection = _zPositionTrackBar.Value / _positionSpeed;
      foreach (var scene in _scene) {
        scene.AddComponent(new CNewLightPosition(new Vector3(xDirection, yDirection, zDirection)));
      }
    }

    private void CreateRenderForm() {

      _renderForm = new RenderForm();
      Renderer.GetRenderer(_renderForm.Handle);

      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.KeyPreview = true;
      _renderForm.AllowUserResizing = false;
      _renderForm.SuspendLayout();
      _renderForm.Name = "MyForm";
      _renderForm.MouseDown += new MouseEventHandler(MyFormMouseDown);
      _renderForm.MouseMove += new MouseEventHandler(MyFormMouseMove);
      _renderForm.MouseUp += new MouseEventHandler(MyFormMouseUp);
      _renderForm.ResumeLayout(false);
      _renderForm.KeyPress += new KeyPressEventHandler(MyFormKeyPress);
    }

    private void MyFormKeyPress(object sender, KeyPressEventArgs e) {
      foreach (var scene in _scene) {
        if (e.KeyChar == 'w') {
          scene.AddComponent(new CFwdCameraMoving());
        } else if (e.KeyChar == 's') {
          scene.AddComponent(new CBackCameraMoving());
        } else if (e.KeyChar == 'd') {
          scene.AddComponent(new CRightCameraMoving());
        } else if (e.KeyChar == 'a') {
          scene.AddComponent(new CLeftCameraMoving());
        } else if (e.KeyChar == '=') {
          scene.AddComponent(new CUpCameraMoving());
        } else if (e.KeyChar == '-') {
          scene.AddComponent(new CDownCameraMoving());
        }
      }
    }

    private void MyFormMouseMove(object sender, MouseEventArgs e) {

      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      var deltaX = mouseArgs.X - _mouse.X;
      var deltaY = mouseArgs.Y - _mouse.Y;
      _mouse.X = mouseArgs.X;
      _mouse.Y = mouseArgs.Y;

      foreach (var scene in _scene) {
        if (_isRotationDown) {
          scene.AddComponent(new CPitch((float)deltaY / _rotationSpeed));
          scene.AddComponent(new CYaw((float)deltaX / _rotationSpeed));
        } else if (_isMouseDown) {

          if (deltaX > 0) {
            scene.AddComponent(new CLeftCameraMoving());
          } else {
            scene.AddComponent(new CRightCameraMoving());
          }

          if (deltaY > 0) {
            scene.AddComponent(new CUpCameraMoving());
          } else {
            scene.AddComponent(new CDownCameraMoving());
          }
        }
      }
    }

    private void MyFormMouseDown(object sender, MouseEventArgs e) {

      MouseEventArgs mouseArgs = e;
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
