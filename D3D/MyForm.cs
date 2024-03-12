using System;
using System.Drawing;
using System.Windows.Forms;
using D3D;
using SharpDX;
using SharpDX.Windows;

using Color = SharpDX.Color;
namespace D3D {

  public class MyForm : RenderForm, IDisposable {



    bool isMouseDown = false;

    private const int Width = 800;
    private const int Height = 600;

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

      _vertices[0] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[1] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[2] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[3] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));

      _vertices[4] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[5] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[6] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));
      _vertices[7] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 1.0f, 1.0f));

      _vertices[8] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[9] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[10] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[11] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[12] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(1.0f, 0.0f, 0.0f, 1.0f));
      _vertices[13] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(1.0f, 0.0f, 0.0f, 1.0f));
      _vertices[14] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(1.0f, 0.0f, 0.0f, 1.0f));
      _vertices[15] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(1.0f, 0.0f, 0.0f, 1.0f));

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

      _renderForm = new RenderForm();

      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;

      _renderer = new Renderer(_renderForm.Handle);

      _buttonDown = new Button();
      _buttonDown.Text = "down";
      _buttonDown.Size = new Size(50, 25);
      _buttonDown.Location = new System.Drawing.Point(10, 10);
      _buttonDown.Click += OnButtonDownClick;
      _renderForm.Controls.Add(_buttonDown);

      _buttonUp = new Button();
      _buttonUp.Text = "up";
      _buttonUp.Size = new Size(50, 25);
      _buttonUp.Location = new System.Drawing.Point(10, 40);
      _buttonUp.Click += OnButtonUpClick;
      _renderForm.Controls.Add(_buttonUp);

      _buttonRight = new Button();
      _buttonRight.Text = "right";
      _buttonRight.Size = new Size(50, 25);
      _buttonRight.Location = new System.Drawing.Point(10, 70);
      _buttonRight.Click += OnButtonRightClick;
      _renderForm.Controls.Add(_buttonRight);

      _buttonLeft = new Button();
      _buttonLeft.Text = "left";
      _buttonLeft.Size = new Size(50, 25);
      _buttonLeft.Location = new System.Drawing.Point(10, 100);
      _buttonLeft.Click += OnButtonLeftClick;
      _renderForm.Controls.Add(_buttonLeft);



      _buttonMoveLeft = new Button();
      _buttonMoveLeft.Text = "move left";
      _buttonMoveLeft.Size = new Size(100, 25);
      _buttonMoveLeft.Location = new System.Drawing.Point(100, 10);
      _buttonMoveLeft.Click += OnButtonMoveLeftClick;
      _renderForm.Controls.Add(_buttonMoveLeft);

      _buttonMoveRight = new Button();
      _buttonMoveRight.Text = "move right";
      _buttonMoveRight.Size = new Size(100, 25);
      _buttonMoveRight.Location = new System.Drawing.Point(100, 40);
      _buttonMoveRight.Click += OnButtonMoveRightClick;
      _renderForm.Controls.Add(_buttonMoveRight);

      _buttonMoveDown = new Button();
      _buttonMoveDown.Text = "move down";
      _buttonMoveDown.Size = new Size(100, 25);
      _buttonMoveDown.Location = new System.Drawing.Point(100, 70);
      _buttonMoveDown.Click += OnButtonMoveUpClick;
      _renderForm.Controls.Add(_buttonMoveDown);

      _buttonMoveUp = new Button();
      _buttonMoveUp.Text = "move up";
      _buttonMoveUp.Size = new Size(100, 25);
      _buttonMoveUp.Location = new System.Drawing.Point(100, 100);
      _buttonMoveUp.Click += OnButtonMoveDownClick;
      _renderForm.Controls.Add(_buttonMoveUp);

    }

    void OnButtonMoveLeftClick(object sender, EventArgs e) {
      _renderer.MoveCameraLeftRight(-0.2f);
    }

    void OnButtonMoveRightClick(object sender, EventArgs e) {
      _renderer.MoveCameraLeftRight(0.2f);
    }

    void OnButtonMoveUpClick(object sender, EventArgs e) {
      _renderer.MoveCameraUpDown(-0.2f);
    }

    void OnButtonMoveDownClick(object sender, EventArgs e) {
      _renderer.MoveCameraUpDown(0.2f);
    }


    void OnButtonDownClick(Object sender, EventArgs e) {
      _renderer.ChangePitch(5.0f);
    }

    void OnButtonUpClick(Object sender, EventArgs e) {
      _renderer.ChangePitch(-5.0f);
    }

    void OnButtonLeftClick(Object sender, EventArgs e) {
      _renderer.ChangeYaw(-5.0f);
    }

    void OnButtonRightClick(Object sender, EventArgs e) {
      _renderer.ChangeYaw(5.0f);
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

    private void InitializeComponent() {
      this.SuspendLayout();
      // 
      // MyForm
      // 
      this.ClientSize = new System.Drawing.Size(800, 600);
      this.Name = "MyForm";
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseDown);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseMove);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MyForm_MouseUp);
      this.ResumeLayout(false);

    }

    private void MyForm_MouseMove(object sender, MouseEventArgs e) {
      MouseEventArgs mouseArgs = (MouseEventArgs)e;
      int mouseX = mouseArgs.X;
      int mouseY = mouseArgs.Y;
    }

    private void MyForm_MouseDown(object sender, MouseEventArgs e) {
      isMouseDown = true;
    }

    private void MyForm_MouseUp(object sender, MouseEventArgs e) {
      isMouseDown = false;
    }
  }
}
