using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace app
{
    public class Window : GameWindow
    {

        private readonly float[] vertices = {
    -0.5f, -0.5f, 0.0f,
     0.5f, -0.5f, 0.0f,
     0.0f,  0.5f, 0.0f
    };

        private int _vertex_buffer_object;
        private int _vertex_array_object;
        private app.Shader _shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        // Any initialization-related code
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertex_buffer_object = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertex_buffer_object);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _vertex_array_object = GL.GenVertexArray();
            GL.BindVertexArray(_vertex_array_object);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _shader = new app.Shader("C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.vert",
                "C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.frag");

            _shader.Use();

        }


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            GL.BindVertexArray(_vertex_array_object);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }


        protected override void OnUnload()
        {
            base.OnUnload();
            _shader.Dispose();
        }
    }
}
