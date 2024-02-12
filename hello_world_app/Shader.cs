using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace app {
  public class Shader {
    public int Handle { get; init; }
    private bool _disposedValue = false;
    private readonly Dictionary<string, int> _uniformLocations;

    public Shader(string vertexPath, string fragmentPath) {
      string VertexShaderSource = File.ReadAllText(vertexPath);
      string FragmentShaderSource = File.ReadAllText(fragmentPath);

      int VertexShader = GL.CreateShader(ShaderType.VertexShader);
      int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);

      GL.ShaderSource(VertexShader, VertexShaderSource);
      GL.ShaderSource(FragmentShader, FragmentShaderSource);

      GL.CompileShader(VertexShader);
      GL.CompileShader(FragmentShader);

      Handle = GL.CreateProgram();

      GL.AttachShader(Handle, VertexShader);
      GL.AttachShader(Handle, FragmentShader);

      GL.LinkProgram(Handle);

      GL.DetachShader(Handle, VertexShader);
      GL.DetachShader(Handle, FragmentShader);
      GL.DeleteShader(VertexShader);
      GL.DeleteShader(FragmentShader);

      GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numOfUniforms);
      _uniformLocations = new Dictionary<string, int>();

      for (var i = 0; i < numOfUniforms; ++i) {
        var key = GL.GetActiveUniform(Handle, i, out _, out _);
        var location = GL.GetUniformLocation(Handle, key);
        _uniformLocations.Add(key, location);
      }
    }

    public void Use() {
      GL.UseProgram(Handle);
    }

    public void SetInt(string name, int num) {
      GL.UseProgram(Handle);
      GL.Uniform1(_uniformLocations[name], num);
    }

    public void SetMatrix4(string name, Matrix4 data) {
      GL.UseProgram(Handle);
      GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }


    protected virtual void Dispose(bool disposing) {
      if (!_disposedValue) {
        GL.DeleteProgram(Handle);
        _disposedValue = true;
      }
    }

    ~Shader() {
      if (_disposedValue == false) {
        Console.WriteLine("GPU Leak");
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
