using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("abc");

        using (Game game = new Game(800, 600, "hello"))
        {
            game.Run();
        }
    }

}

public class Game : GameWindow
{

    float[] vertices = {
    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
     0.5f, -0.5f, 0.0f, //Bottom-right vertex
     0.0f,  0.5f, 0.0f  //Top vertex
    };
    int vertex_buffer_object;
    Shader shader;

    public Game(int w, int h, string title) : base(GameWindowSettings.Default, new NativeWindowSettings()
    { Size = (w, h), Title = title }) { }


    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    // Any initialization-related code
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        vertex_buffer_object = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_object);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        shader = new Shader("C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.vert", 
            "C:\\Users\\alex\\source\\repos\\Alex-Kalinin-jr\\internature\\hello_world_app\\shader.frag");
    }

    // copies user-defined data into the currently-bound buffer
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload() {
        base.OnUnload();
        shader.Dispose();
    }
}

public class Shader
{
    int Handle;
    private bool disposedValue = false;


    public Shader(string vertexPath, string fragmentPath)
    {
        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexShader, VertexShaderSource);
        GL.ShaderSource (FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            Console.WriteLine(GL.GetShaderInfoLog(VertexShader));
        } else
        {
            Console.WriteLine("Vertex shader was compiled");
        }

        GL.CompileShader(FragmentShader);
        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            Console.WriteLine(GL.GetShaderInfoLog(FragmentShader));
        } else
        {
            Console.WriteLine("Fragment shader was compiled");
        }

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);
        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        } else
        {
            Console.WriteLine("shader was integrated");
        }

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(VertexShader);
        GL.DeleteShader(FragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    protected virtual void Dispose(bool disposing) 
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Leak");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}