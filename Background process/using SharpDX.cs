using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.DXGI;
using System;
using SharpDX.D3DCompiler;
using SharpDX.Windows;


namespace D3D {
  using Buffer = SharpDX.Direct3D11.Buffer;
  using Device = SharpDX.Direct3D11.Device;
  using DeviceContext = SharpDX.Direct3D11.DeviceContext;

  internal class Renderer : IDisposable {
    IntPtr ControlHandle { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    private Device _device3D;
    private SwapChain _swapChain;
    private DeviceContext _context3D;
    private RenderTargetView _renderTargetView;

    private VertexShader _vertexShader;
    private PixelShader _pixelShader;

    private Buffer _vertexBuffer;
    private Buffer _constantBuffer;

    private ShaderSignature _inputSignature;
    private InputLayout _inputLayout;

    private Color _background;
    private Vertex[] _vertices;

    public Renderer(IntPtr ptr, int Width, int Height) {
      ControlHandle = ptr;
      _background = Color.Blue;

      InitializeDeviceResources();
      InitializeShaders();
    }

    private void InitializeDeviceResources() {

      var tmpDescription = new ModeDescription(Width, 
                                               Height, 
                                               new Rational(60, 1), 
                                               Format.R8G8B8A8_UNorm);

      var description = new SwapChainDescription {BufferCount = 1,
                                                  ModeDescription = tmpDescription,
                                                  IsWindowed = true,
                                                  OutputHandle = ControlHandle,
                                                  SampleDescription = new SampleDescription(1, 0),
                                                  SwapEffect = SwapEffect.Discard,
                                                  Usage = Usage.RenderTargetOutput
      };

      Device.CreateWithSwapChain(DriverType.Hardware,
                                 DeviceCreationFlags.None, 
                                 description, 
                                 out _device3D, 
                                 out _swapChain);

      _context3D = _device3D.ImmediateContext;

      using (Texture2D backBuffer = _swapChain.GetBackBuffer<Texture2D>(0)) {

        _renderTargetView = new RenderTargetView(_device3D, backBuffer);

      }

      var viewport = new Viewport(0, 0, Width, Height);

      _context3D.Rasterizer.SetViewport(viewport);

    }

    private void InitializeShaders() {

      using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile(
          "Shaders/VertexShader.hlsl", "main", "vs_4_0", ShaderFlags.Debug)) {
        _inputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
        _vertexShader = new VertexShader(_device3D, vertexShaderByteCode);
      }

      using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile(
          "Shaders/PixelShader.hlsl", "main", "ps_4_0", ShaderFlags.Debug)) {
        _pixelShader = new PixelShader(_device3D, pixelShaderByteCode);
      }

      _context3D.VertexShader.Set(_vertexShader);
      _context3D.PixelShader.Set(_pixelShader);

      _inputLayout = new InputLayout(_device3D, _inputSignature, new[] {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, InputClassification.PerVertexData, 0)
      });
      _context3D.InputAssembler.InputLayout = _inputLayout;

      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
    }

    public void Render(ref VS_CONSTANT_BUFFER tmp, ref Vertex[] vertices) {
      _vertices = new Vertex[] {
        new Vertex(new Vector3(-0.5f, 0.0f, 0.0f), SharpDX.Color.Red),
        new Vertex(new Vector3(0.0f, 0.5f, 0.0f), SharpDX.Color.Green),
        new Vertex(new Vector3(0.5f, -0.0f, 0.0f), SharpDX.Color.Blue),
      };


      _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref tmp);
      _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);



      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, _vertices);

      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);
      _context3D.ClearRenderTargetView(_renderTargetView, _background);
      _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));

      _context3D.Draw(3, 0);

      _swapChain.Present(1, PresentFlags.None);
    }


    public void Dispose() {

      _inputLayout.Dispose();
      _inputSignature.Dispose();
      _vertexBuffer.Dispose();
      _vertexShader.Dispose();
      _pixelShader.Dispose();
      _renderTargetView.Dispose();
      _swapChain.Dispose();
      _device3D.Dispose();
      _context3D.Dispose();

    }


  }

}



using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using Vector3 = SharpDX.Vector3;
using D3D.Entities;

namespace D3D {

  struct VS_CONSTANT_BUFFER {
    public Vector4 cl;
    public Matrix vpMatrix;
  }


  public class MyForm : IDisposable {
    private const int Width = 800;
    private const int Height = 600;

    private RenderForm _renderForm;
    private Camera _camera;
    private Renderer _renderer;

    private Vertex[] _vertices;

    public MyForm() {

      _renderForm = new RenderForm();
      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;

      _camera = new Camera(new Vector3(0.0f, 0.0f, -2.0f), (float)Width / Height);

      _renderer = new Renderer(_renderForm.Handle, Width, Height);

      _vertices = new Vertex[] {
        new Vertex(new Vector3(-0.5f, 0.0f, 0.0f), SharpDX.Color.Red),
        new Vertex(new Vector3(0.0f, 0.5f, 0.0f), SharpDX.Color.Green),
        new Vertex(new Vector3(0.5f, -0.0f, 0.0f), SharpDX.Color.Blue),
      };


    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////


    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RenderCallback() {
      Matrix world = Matrix.Identity;
      float aspectRatio = (float)Width / Height;
      Vector3 eyePos = new Vector3(0.0f, 0.0f, -2.0f);
      Vector3 lookAtPos = new Vector3(0.0f, 0.0f, 0.0f);

      Vector3 upVector = new Vector3(0.0f, 1.0f, 0.0f);
      Matrix view = Matrix.LookAtLH(eyePos, lookAtPos, upVector);

      float fovDegrees = 90.0f;
      float fovRadians = (fovDegrees / 360.0f) * MathUtil.Pi * 2;
      float nearZ = 0.1f;
      float farZ = 1000.0f;
      Matrix projection = Matrix.PerspectiveFovLH(fovRadians, aspectRatio, nearZ, farZ);


      

      VS_CONSTANT_BUFFER tmp = new VS_CONSTANT_BUFFER();
      tmp.cl = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
      //tmp.vpMatrix = world * view * projection;
      //tmp.vpMatrix = world * _camera.GetViewMatrix() * _camera.GetProjectionMatrix();
      tmp.vpMatrix.Transpose();

      tmp.vpMatrix = Matrix.Identity;

      /*
      // Update the model transform buffer for the hologram.
      context->UpdateSubresource(
          m_modelConstantBuffer.Get(),
          0,
          nullptr,
          &m_modelConstantBufferData,
          0,
          0
      );
      */
      _renderer.Render(ref tmp, ref _vertices);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Dispose() {
      _renderer.Dispose();
      _renderForm.Dispose();
    }

  }
}
