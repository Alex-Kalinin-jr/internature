using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;
using Assimp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace D3D {

  public class MyForm : RenderForm {

    new private const int Width = 1024;
    new private const int Height = 768;

    private CMousePos _mouse;
    private List<Scene> _scene;

    private RenderForm _renderForm;

    private CPositionTrackBar _xPositionTrackBar;
    private CPositionTrackBar _yPositionTrackBar;
    private CPositionTrackBar _zPositionTrackBar;

    private bool _isMouseDown = false;
    private bool _isRotationDown = false;

    private const float _rotationSpeed = 10.0f;
    private const float _positionSpeed = 20.0f;

    public MyForm() {
      CreateRenderForm();

      _mouse = new CMousePos();
      _scene = new List<Scene> {Generator.CreateGridTestingScene(), Generator.CreateTestingScene()};

      string text = "W - move forward\nA - move left\nS - move backward\nD - move right\n= - move up\n- - move down\nRMB - movings\nWheel-Pressed - rotation";
      new CLabel(_renderForm, new System.Drawing.Point(25, 25), text);

      var button = new CButton(_renderForm, new System.Drawing.Point(25, 300), "light color");
      button.IamButton.Click += ChangeLightColor;

      new CLabel(_renderForm, new System.Drawing.Point(25, 160), "x-coord");
      _xPositionTrackBar = new CPositionTrackBar(_renderForm, new System.Drawing.Point(100, 150));
      _xPositionTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

      new CLabel(_renderForm, new System.Drawing.Point(25, 210), "y-coord");
      _yPositionTrackBar = new CPositionTrackBar(_renderForm, new System.Drawing.Point(100, 200));
      _yPositionTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

      new CLabel(_renderForm, new System.Drawing.Point(25, 260), "z-coord");
      _zPositionTrackBar = new CPositionTrackBar(_renderForm, new System.Drawing.Point(100, 250));
      _zPositionTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

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

    private void ChangeLightPosition(object sender, EventArgs e) {
      float xDirection = _xPositionTrackBar.IamTrackBar.Value / _positionSpeed;
      float yDirection = _yPositionTrackBar.IamTrackBar.Value / _positionSpeed;
      float zDirection = _zPositionTrackBar.IamTrackBar.Value / _positionSpeed;
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
