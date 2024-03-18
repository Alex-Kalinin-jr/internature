using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;


namespace D3D {

  public class MyForm : RenderForm {

    private List<Scene> _scene;

    private CRenderForm _renderForm;
    private Layout _layout;

    private CPositionTrackBar _xPositionTrackBar;
    private CPositionTrackBar _yPositionTrackBar;
    private CPositionTrackBar _zPositionTrackBar;

    private bool _isMouseDown = false;
    private bool _isRotationDown = false;
    private CMouseMovingParams _movingParams;

    public MyForm() {
      _layout = new Layout();
      var renderForm = _layout.GetComponent<CRenderForm>().IamRenderForm;
      Renderer.GetRenderer(renderForm.Handle);
      renderForm.MouseDown += new MouseEventHandler(MyFormMouseDown);
      renderForm.MouseMove += new MouseEventHandler(MyFormMouseMove);
      renderForm.MouseUp += new MouseEventHandler(MyFormMouseUp);
      renderForm.KeyPress += new KeyPressEventHandler(MyFormKeyPress);

      _scene = new List<Scene> {Generator.CreateGridTestingScene(), Generator.CreateTestingScene()};

      _movingParams = new CMouseMovingParams(10.0f, 20.0f);

    }

    public void Run() {
      RenderLoop.Run(_renderForm.IamRenderForm, RenderCallback);
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
      float xDirection = _xPositionTrackBar.IamTrackBar.Value / _movingParams.IamShiftDivider;
      float yDirection = _yPositionTrackBar.IamTrackBar.Value / _movingParams.IamShiftDivider;
      float zDirection = _zPositionTrackBar.IamTrackBar.Value / _movingParams.IamShiftDivider;
      foreach (var scene in _scene) {
        scene.AddComponent(new CNewLightPosition(new Vector3(xDirection, yDirection, zDirection)));
      }
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
          scene.AddComponent(new CPitch(deltaY / _movingParams.IamRotDivider));
          scene.AddComponent(new CYaw(deltaX / _movingParams.IamRotDivider));
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
