using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using System.Collections.Generic;


using Color = SharpDX.Color;
namespace D3D {



  public class MyForm : RenderForm, IDisposable {
    public struct MousePos {
      public int X;
      public int Y;

      public MousePos(int x = 0, int y = 0) {
        X = x;
        Y = y;
      }
    }

    private const int Width = 800;
    private const int Height = 600;

    private MousePos _mouse;

    private RenderForm _renderForm;
    private Renderer _renderer;

    private List<Mesh> _mesh;

    bool _isMouseDown = false;
    bool _isRotationDown = false;

    public MyForm() {

      _mesh = new List<Mesh>();
      _mesh.Add(new Mesh("Models/mitsuba-sphere.obj"));

      _mouse = new MousePos();

      _renderForm = new RenderForm();

      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;
      _renderForm.SuspendLayout();
      _renderForm.Name = "MyForm";
      _renderForm.MouseDown += new MouseEventHandler(this.MyForm_MouseDown);
      _renderForm.MouseMove += new MouseEventHandler(this.MyForm_MouseMove);
      _renderForm.MouseUp += new MouseEventHandler(this.MyForm_MouseUp);
      _renderForm.ResumeLayout(false);

      _renderForm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MyForm_KeyPress);

      _renderer = new Renderer(_renderForm.Handle);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MyForm_KeyPress(object sender, KeyPressEventArgs e) {
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

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RenderCallback() {
      foreach (var form in _mesh) {
        _renderer.RenderCallback(form.Vertices.ToArray(), form.Indices.ToArray());
      }
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Dispose() {

      _renderForm.Dispose();
      _renderer.Dispose();

    }

    private void MyForm_MouseMove(object sender, MouseEventArgs e) {

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

    private void MyForm_MouseDown(object sender, MouseEventArgs e) {

      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      _mouse.X = mouseArgs.X;
      _mouse.Y = mouseArgs.Y;

      if (mouseArgs.Button == MouseButtons.Left) {
        _isMouseDown = true;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        _isRotationDown = true;
      }

    }

    private void MyForm_MouseUp(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      if (mouseArgs.Button == MouseButtons.Left) {
        _isMouseDown = false;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        _isRotationDown = false;
      }
    }
  }
}
