using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX;


namespace D3D {

  public class MyForm : RenderForm {

    private List<Scene> _scene;
    private Layout _layout;
    private CMouseMovingParams _movingParams;

    private bool _isMouseDown = false;
    private bool _isRotationDown = false;


    public MyForm() {

      _layout = new Layout();
      _scene = new List<Scene> {Generator.CreateGridTestingScene(),
                                Generator.CreateAnotherPipeTestingScene()};
      _movingParams = new CMouseMovingParams(10.0f, 20.0f);

      var renderForm = _layout.GetComponent<CRenderForm>().IamRenderForm;

      var trackBar = _layout.GetComponent<CPositionTrackBar>();
      trackBar.IamXTrackBar.Scroll += ChangeLightPosition;
      trackBar.IamYTrackBar.Scroll += ChangeLightPosition;
      trackBar.IamZTrackBar.Scroll += ChangeLightPosition;

      var cliceBar = _layout.GetComponent<CCliceTrackBar>();
      cliceBar.IamXTrackBar.Scroll += CliceGrid;
      cliceBar.IamYTrackBar.Scroll += CliceGrid;
      cliceBar.IamZTrackBar.Scroll += CliceGrid;


      var radioButton = _layout.GetComponent<CRadioButton>();
      radioButton.RadioButton1.CheckedChanged += ChangePipeShowType;

      var button = _layout.GetComponent<CButton>();
      button.IamButton.Click += ChangeLightColor;

      Renderer.GetRenderer(renderForm.Handle);
      renderForm.MouseDown += new MouseEventHandler(MyFormMouseDown);
      renderForm.MouseMove += new MouseEventHandler(MyFormMouseMove);
      renderForm.MouseUp += new MouseEventHandler(MyFormMouseUp);
      renderForm.KeyPress += new KeyPressEventHandler(MyFormKeyPress);

    }


    public void Run() {
      var form = _layout.GetComponent<CRenderForm>();
      RenderLoop.Run(form.IamRenderForm, RenderCallback);
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
        var color = new Vector4(buff.Color.R / 255, buff.Color.G / 255, 
                                buff.Color.B / 255, buff.Color.A / 255);
        LightSystem.ChangeColor(color);
      }
    }


    private void ChangeLightPosition(object sender, EventArgs e) {
      var trackBar = _layout.GetComponent<CPositionTrackBar>();
      float xDirection = trackBar.IamXTrackBar.Value / _movingParams.IamShiftDivider;
      float yDirection = trackBar.IamYTrackBar.Value / _movingParams.IamShiftDivider;
      float zDirection = trackBar.IamZTrackBar.Value / _movingParams.IamShiftDivider;
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
      var mouse = _layout.GetComponent<CMousePos>();
      var deltaX = mouseArgs.X - mouse.X;
      var deltaY = mouseArgs.Y - mouse.Y;
      mouse.X = mouseArgs.X;
      mouse.Y = mouseArgs.Y;

      foreach (var scene in _scene) {
        if (_isRotationDown) {
          scene.AddComponent(new CPitch(deltaY / _movingParams.IamRotDivider));
          scene.AddComponent(new CYaw(deltaX / _movingParams.IamRotDivider));
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
      var mouse = _layout.GetComponent<CMousePos>();
      mouse.X = mouseArgs.X;
      mouse.Y = mouseArgs.Y;

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

    private void CliceGrid(object sender, EventArgs e) {
      var trackBar = _layout.GetComponent<CCliceTrackBar>();
      var x = trackBar.IamXTrackBar.Value;
      var y = trackBar.IamYTrackBar.Value;
      var z = trackBar.IamZTrackBar.Value;
      DrawSystem.CliceGrid(new Vector3(x, y, z));
    }
  }
}
