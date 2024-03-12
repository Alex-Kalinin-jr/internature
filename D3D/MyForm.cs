using System;
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

    private Button _buttonLeft;
    private Button _buttonRight;
    private Button _buttonUp;
    private Button _buttonDown;
    private Button _buttonMoveLeft;
    private Button _buttonMoveRight;
    private Button _buttonMoveUp;
    private Button _buttonMoveDown;

    public MyForm() {

      _vertices = new Vertex[24];

      _vertices[0] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[1] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[2] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[3] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[4] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[5] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[6] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[7] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));

      _vertices[8] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[9] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[10] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[11] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[12] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[13] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[14] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[15] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[16] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[17] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[18] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[19] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[20] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[21] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[22] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[23] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));


      _indices = new short[]
      {
                // front face
                0, 1, 2, // first triangle
                0, 3, 1, // second triangle

                // left face
                4, 5, 6, // first triangle
                4, 7, 5, // second triangle

                // right face
                8, 9, 10, // first triangle
                8, 11, 9, // second triangle

                // back face
                12, 13, 14, // first triangle
                12, 15, 13, // second triangle

                // top face
                16, 17, 18, // first triangle
                16, 19, 17, // second triangle

                // bottom face
                20, 21, 22, // first triangle
                20, 23, 21, // second triangle
      };

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

      _renderer = new Renderer(_renderForm.Handle);

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
      } else if (mouseArgs.Button == MouseButtons.Right) {
        isRotationDown = true;
      }

    }

    private void MyForm_MouseUp(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      if (mouseArgs.Button == MouseButtons.Left) {
        isMouseDown = false;
      } else if (mouseArgs.Button == MouseButtons.Right) {
        isRotationDown = false;
      }
    }
  }
}
