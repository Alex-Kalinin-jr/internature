﻿using System;
using System.Drawing;
using System.Windows.Forms;
using D3D;
using SharpDX;
using SharpDX.Windows;

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

    bool isMouseDown = false;
    bool isRotationDown = false;

    private const int Width = 800;
    private const int Height = 600;

    private MousePos _mouse;

    private RenderForm _renderForm;
    private Renderer _renderer;
    private Vertex[] _vertices;
    private short[] _indices;


    public MyForm() {

      (_vertices, _indices) = Generator.GenerateCube();

      _mouse = new MousePos();

      _renderForm = new RenderForm();

      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;
      _renderForm.SuspendLayout();
      _renderForm.Name = "MyForm";
      _renderForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseDown);
      _renderForm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseMove);
      _renderForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseUp);
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
      _renderer.RenderCallback(_vertices, _indices);
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
      if (isRotationDown) {
        _renderer.ChangePitch(deltaY / 10.0f);
        _renderer.ChangeYaw(deltaX / 10.0f);
        _mouse.X = mouseArgs.X;
        _mouse.Y = mouseArgs.Y;
      } else if (isMouseDown) {
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
        isMouseDown = true;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        isRotationDown = true;
      }

    }

    private void MyForm_MouseUp(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      if (mouseArgs.Button == MouseButtons.Left) {
        isMouseDown = false;
      } else if (mouseArgs.Button == MouseButtons.Middle) {
        isRotationDown = false;
      }
    }
  }
}
