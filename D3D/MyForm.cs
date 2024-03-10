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
using SharpDX.D3DCompiler;

namespace D3D {

  struct VS_CONSTANT_BUFFER {
    public Vector4 cl;
  }




  public class MyForm : IDisposable {
    private const int Width = 800;
    private const int Height = 600;

    private RenderForm _renderForm;

    private Device _device3D;
    private SwapChain _swapChain;
    private DeviceContext _context3D;
    private RenderTargetView _renderTargetView;

    private Vertex[] _vertices;

    private Buffer _vertexBuffer;
    private Buffer _constantBuffer;

    private VertexShader _vertexShader;
    private PixelShader _pixelShader;

    // test
    private SharpDX.Color _background = SharpDX.Color.White;
    // test

    private ShaderSignature _inputSignature;
    private InputLayout _inputLayout;

    public MyForm() {

      _renderForm = new RenderForm();
      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;

      InitializeDeviceResources();
      InitializeShaders();
      InitializeBuffers();

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeDeviceResources() {
      var description = new SwapChainDescription {
        BufferCount = 1,
        ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
        IsWindowed = true,
        OutputHandle = _renderForm.Handle,
        SampleDescription = new SampleDescription(1, 0),
        SwapEffect = SwapEffect.Discard,
        Usage = Usage.RenderTargetOutput
      };

      Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out _device3D, out _swapChain);
      _context3D = _device3D.ImmediateContext;

      using (Texture2D backBuffer = _swapChain.GetBackBuffer<Texture2D>(0)) {
        _renderTargetView = new RenderTargetView(_device3D, backBuffer);
      }

      var viewport = new Viewport(0, 0, Width, Height);
      _context3D.Rasterizer.SetViewport(viewport);

    }
    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void InitializeBuffers() {
      _vertices = new Vertex[] {
        new Vertex(new Vector3(-0.5f, 0.0f, 0.0f), SharpDX.Color.Red),
        new Vertex(new Vector3(0.0f, 0.5f, 0.0f), SharpDX.Color.Green),
        new Vertex(new Vector3(0.5f, -0.0f, 0.0f), SharpDX.Color.Blue),
      };
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
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

      _inputLayout = new InputLayout(_device3D, _inputSignature, new [] {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, InputClassification.PerVertexData, 0)
      });
      _context3D.InputAssembler.InputLayout = _inputLayout;

      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;


    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RenderCallback() {
      VS_CONSTANT_BUFFER tmp = new VS_CONSTANT_BUFFER();
      tmp.cl = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
      _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref tmp);
      _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);


      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, _vertices);

      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);
      _context3D.ClearRenderTargetView(_renderTargetView, _background);
      _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));


      _context3D.Draw(3, 0);

      _swapChain.Present(1, PresentFlags.None);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
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
      _renderForm.Dispose();

    }

  }
}
