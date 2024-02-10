using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Timers;

namespace app {
  public class Window : GameWindow {

    float[] vertices =
    {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

    private readonly uint[] _indices =
    {
            0, 1, 3,
            1, 2, 3
        };

    private int _vertex_buffer_object;
    private int _vertex_array_object;
    private int _element_buffer_object;
    private app.Shader _shader;
    private app.Texture _texture;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings) { }

    protected override void OnLoad() {
      base.OnLoad();
      GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

      _vertex_array_object = GL.GenVertexArray();
      GL.BindVertexArray(_vertex_array_object);

      _vertex_buffer_object = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertex_buffer_object);
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

      _element_buffer_object = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _element_buffer_object);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _shader = new app.Shader("Shaders/shader.vert", "Shaders/shader.frag");
      _shader.Use();

      int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      int texCoordLocation = GL.GetAttribLocation(_shader.Handle, "aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture = Texture.LoadFromFile("Resources/container.png");
      _texture.Use(TextureUnit.Texture0);

    }


    protected override void OnUpdateFrame(FrameEventArgs args) {
      base.OnUpdateFrame(args);

      if (KeyboardState.IsKeyDown(Keys.Escape)) {
        Close();
      }
    }


    protected override void OnRenderFrame(FrameEventArgs args) {
      base.OnRenderFrame(args);
      GL.Clear(ClearBufferMask.ColorBufferBit);

      GL.BindVertexArray(_vertex_array_object);

      _shader.Use();
      _texture.Use(TextureUnit.Texture0);

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
      SwapBuffers();
    }


    protected override void OnResize(ResizeEventArgs e) {
      base.OnResize(e);
      GL.Viewport(0, 0, e.Width, e.Height);
    }


    protected override void OnUnload() {
      base.OnUnload();
      _shader.Dispose();
    }
  }
}
