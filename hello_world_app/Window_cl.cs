using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Timers;

namespace app
{
    public class Window : GameWindow
    {

        private readonly float[] vertices =
        {
          0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
         -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
          0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
        };

        private int _vertex_buffer_object;
        private int _vertex_array_object;
        private int _element_buffer_object;
        private app.Shader _shader;
        private Stopwatch _timer;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertex_buffer_object = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertex_buffer_object);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _vertex_array_object = GL.GenVertexArray();
            GL.BindVertexArray(_vertex_array_object);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // _element_buffer_object = GL.GenBuffer();
            // GL.BindBuffer(BufferTarget.ElementArrayBuffer, _element_buffer_object);
            // GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw); 

            _shader = new app.Shader("C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.vert",
                "C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.frag");

            _shader.Use();

            // _timer = new Stopwatch();
            // _timer.Start();

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

            // varying color
            // double timeValue = _timer.Elapsed.TotalSeconds;
            // float green_val = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            // float red_val = (float)Math.Cos(timeValue) / 2.0f + 0.3f;
            // float blue_val = (float)Math.Tan(timeValue) / 2.0f + 0.4f;
            // int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            // GL.Uniform4(vertexColorLocation, red_val, green_val, 0.0f, 1.0f);
            // end of varying color

            GL.BindVertexArray(_vertex_array_object);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            // GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
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
