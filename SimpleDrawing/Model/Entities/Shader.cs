using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class Shader {

    public readonly Dictionary<string, int> UniformLocations;

    public int Handle { get; init; }
    private bool _disposedValue = false;

    public Shader(string vertexPath, string fragmentPath) {

      string VertexShaderSource = File.ReadAllText(vertexPath);
      string FragmentShaderSource = File.ReadAllText(fragmentPath);

      int VertexShader = GL.CreateShader(ShaderType.VertexShader);
      int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);

      GL.ShaderSource(VertexShader, VertexShaderSource);
      GL.ShaderSource(FragmentShader, FragmentShaderSource);

      CompileShader(VertexShader);
      CompileShader(FragmentShader);

      Handle = GL.CreateProgram();

      GL.AttachShader(Handle, VertexShader);
      GL.AttachShader(Handle, FragmentShader);

      LinkProgram(Handle);

      GL.DetachShader(Handle, VertexShader);
      GL.DetachShader(Handle, FragmentShader);
      GL.DeleteShader(VertexShader);
      GL.DeleteShader(FragmentShader);

      GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numOfUniforms);
      UniformLocations = new Dictionary<string, int>();

      for (var i = 0; i < numOfUniforms; ++i) {
        var key = GL.GetActiveUniform(Handle, i, out _, out _);
        var location = GL.GetUniformLocation(Handle, key);
        UniformLocations.Add(key, location);
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

    protected virtual void Dispose(bool disposing) {
      if (!_disposedValue) {
        GL.DeleteProgram(Handle);
        _disposedValue = true;
      }
    }


    public void Use() {
      GL.UseProgram(Handle);
    }

    public void SetInt(string name, int num) {
      GL.UseProgram(Handle);
      GL.Uniform1(UniformLocations[name], num);
    }

    public void SetFloat(string name, float num) {
      GL.UseProgram(Handle);
      GL.Uniform1(UniformLocations[name], num);
    }

    public void SetMatrix4(string name, Matrix4 data) {
      GL.UseProgram(Handle);
      GL.UniformMatrix4(UniformLocations[name], true, ref data);
    }
    public void SetUniform3(string name, Vector3 data) {
      GL.UseProgram(Handle);
      GL.Uniform3(UniformLocations[name], data);
    }

    private static void CompileShader(int shader) {
      GL.CompileShader(shader);
      GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
      if (code != (int)All.True) {
        var infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
      }
    }

    private static void LinkProgram(int program) {
      GL.LinkProgram(program);
      GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
      if (code != (int)All.True) {
        throw new Exception($"Error occurred whilst linking Program({program})");
      }
    }
  }
}
