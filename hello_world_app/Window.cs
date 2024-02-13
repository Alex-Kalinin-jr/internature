using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Timers;

namespace app {
  public class Window : GameWindow {

    private List<Volume> _volumes = new List<Volume>();
    private List<int> _vertex_buffer_objects = new List<int>();
    private List<int> _vertex_array_objects = new List<int>();
    private List<int> _element_buffer_objects;

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
      Cube buff2 = new Cube();
      _volumes.Add(buff);
      _volumes.Add(buff2);
      buff2.PosVr += new Vector3(0.0f, 0.01f, 0.0f);


      base.OnLoad();
      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      for (int i = 0; i < _volumes.Count; ++i) {
        _vertex_array_objects.Add(GL.GenVertexArray());
        GL.BindVertexArray(_vertex_array_objects[i]);
        _vertex_buffer_objects.Add(GL.GenBuffer());
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertex_buffer_objects[i]);
        GL.BufferData(BufferTarget.ArrayBuffer,
            _volumes[i].Vertices.Length * sizeof(float),
            _volumes[i].Vertices,
            BufferUsageHint.StaticDraw);
        /*
        _element_buffer_objects.Add(GL.GenBuffer());
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _element_buffer_objects[i]);
        GL.BufferData(BufferTarget.ElementArrayBuffer, 
            _volumes[i].Indices.Length * sizeof(uint), 
            _volumes[i].Indices, 
            BufferUsageHint.StaticDraw);
        */
      }



      _shader = new app.Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _shader.Use();

      int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3,
          VertexAttribPointerType.Float, 
          false, 3 * sizeof(float), 0);

      int texCoordLocation = GL.GetAttribLocation(_shader.Handle, "aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2,
          VertexAttribPointerType.Float, 
          false, 2 * sizeof(float), 0);

      _texture = Texture.LoadFromFile("Resources/container.png");
      _texture.Use(TextureUnit.Texture0);

      _shader.SetInt("texture1", 0);

      _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
      CursorState = CursorState.Grabbed;
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void OnRenderFrame(FrameEventArgs e) {

      base.OnRenderFrame(e);

      _time += 4 * (float)e.Time;

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      for (int i = 0; i < _volumes.Count; ++i) {

        GL.BindVertexArray(_vertex_array_objects[i]);
        _texture.Use(TextureUnit.Texture0);
        _shader.Use();

        // here params of object position are changed
        // why so little value give so meaningful effect?????????????????????????????
        /*
        _volumes[i].RotationVr += new Vector3(_time, 0.0f, 0.0f);
        */
        Matrix4 model = _volumes[i].ComputeModelMatrix();


        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", _camera.GetViewMatrix());
        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

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
