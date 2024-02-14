using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace app {
  public class Window : GameWindow {

    private List<Volume> _volumes = new List<Volume>();
    private List<int> _vertexBufferObjects = new List<int>();
    private List<int> _colorBufferObjects = new List<int>();
    private List<int> _vertexArrayObjects = new List<int>();
    private List<int> _elementBufferObjects = new List<int>();

    private app.Shader _shader;
    private app.Texture _texture;
    private app.Camera _camera;

    private bool _firstMove = true;
    private Vector2 _lastPos;
    private float _time;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings) { }

    /// ////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnLoad() {

      Cube buff = new Cube();
      Cube buff2 = new Cube(5);
      buff2.PosVr += new Vector3(3.0f, 0.0f, 0.0f);
      Cube buff3 = new Cube(20);
      buff3.PosVr += new Vector3(6.0f, 0.0f, 0.0f);
      _volumes.Add(buff);   
      _volumes.Add(buff2);
      _volumes.Add(buff3);

      base.OnLoad();
      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      _shader = new app.Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _shader.Use();

      _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
      CursorState = CursorState.Grabbed;

      for (int i = 0; i < _volumes.Count; ++i) {
        _vertexArrayObjects.Add(GL.GenVertexArray());
        GL.BindVertexArray(_vertexArrayObjects[i]);

        if (_volumes[i].Vertices != null) {
          BindPosBuffer(i);
        }

        if (_volumes[i].Colors != null) {
          BindColorBuffer(i);
        }

        if (_volumes[i].Indices != null) {
          BindIndicesBuffer(i);
        }
      }
    }

    private void BindPosBuffer(int indexOfDescriptors) {
      _vertexBufferObjects.Add(GL.GenBuffer());
      int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPosition");
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjects[indexOfDescriptors]);
      GL.BufferData(BufferTarget.ArrayBuffer, 
          _volumes[indexOfDescriptors].Vertices.Length * sizeof(float),
          _volumes[indexOfDescriptors].Vertices, BufferUsageHint.DynamicDraw);
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
          false, 3 * sizeof(float), 0);
    }

    private void BindColorBuffer(int indexOfDescriptors) {
      _colorBufferObjects.Add(GL.GenBuffer());
      int colorLocation = GL.GetAttribLocation(_shader.Handle, "aColor");
      GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObjects[indexOfDescriptors]);
      GL.BufferData(BufferTarget.ArrayBuffer, 
        _volumes[indexOfDescriptors].Colors.Length * sizeof(float),
        _volumes[indexOfDescriptors].Colors, BufferUsageHint.StaticDraw);
      GL.EnableVertexAttribArray(colorLocation);
      GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float,
          false, 3 * sizeof(float), 0);
    }

    private void BindIndicesBuffer(int indexOfDescriptros) {
      _elementBufferObjects.Add(GL.GenBuffer());
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjects[indexOfDescriptros]);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _volumes[indexOfDescriptros].Indices.Length * sizeof(uint),
          _volumes[indexOfDescriptros ].Indices, BufferUsageHint.StaticDraw);
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnRenderFrame(FrameEventArgs e) {

      base.OnRenderFrame(e);

      _time += 4 * (float)e.Time;

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _shader.SetMatrix4("view", _camera.GetViewMatrix()); 
      _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());


      for (int i = 0; i < _volumes.Count; ++i) {

        _shader.Use();
        Matrix4 model = _volumes[i].ComputeModelMatrix();
        _shader.SetMatrix4("model", model);

        GL.BindVertexArray(_vertexArrayObjects[i]);
        if (_volumes[i].Indices != null) {
          GL.DrawElements(PrimitiveType.Triangles, _volumes[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
        } else {
          GL.DrawArrays(PrimitiveType.Triangles, 0, _volumes[i].Vertices.Length / 3);
        }

      }

      SwapBuffers();
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnUpdateFrame(FrameEventArgs args) {
      base.OnUpdateFrame(args);

      if (!IsFocused) {
        return;
      }

      var input = KeyboardState;

      if (input.IsKeyDown(Keys.Escape)) {
        Close();
      }


      const float cameraSpeed = 1.5f;
      const float sensitivity = 0.2f;

      if (input.IsKeyDown(Keys.W)) {
        _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
      }

      if (input.IsKeyDown(Keys.S)) {
        _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
      }

      if (input.IsKeyDown(Keys.A)) {
        _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
      }

      if (input.IsKeyDown(Keys.D)) {
        _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
      }

      if (input.IsKeyDown(Keys.Space)) {
        _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
      }

      if (input.IsKeyDown(Keys.LeftShift)) {
        _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
      }

      var mouse = MouseState;

      if (_firstMove) {
        _lastPos = new Vector2(mouse.X, mouse.Y);
        _firstMove = false;
      } else {
        var deltaX = mouse.X - _lastPos.X;
        var deltaY = mouse.Y - _lastPos.Y;
        _lastPos = new Vector2(mouse.X, mouse.Y);

        _camera.Yaw += deltaX * sensitivity;
        _camera.Pitch -= deltaY * sensitivity;
      }

    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnMouseWheel(MouseWheelEventArgs e) {
      base.OnMouseWheel(e);
      _camera.Fov -= e.OffsetY;
    }



    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnResize(ResizeEventArgs e) {
      base.OnResize(e);
      GL.Viewport(0, 0, Size.X, Size.Y);
      _camera.AspectRatio = Size.X / (float)Size.Y;
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnUnload() {
      base.OnUnload();
      _shader.Dispose();
    }
  }
}
