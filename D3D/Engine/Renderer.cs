using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using SharpDX.D3DCompiler;

namespace D3D {

  public class Renderer : IDisposable {

    private const int Width = 1024;
    private const int Height = 768;

    private IntPtr _formPtr;

    private Device _device3D;
    private SwapChain _swapChain;
    private DeviceContext _context3D;
    private RenderTargetView _renderTargetView;

    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;
    private Buffer _constantBuffer;
    private Buffer _constantLightBuffer;

    private VertexShader _vertexShader;
    private PixelShader _pixelShader;

    private ShaderSignature _inputSignature;
    private InputLayout _inputLayout;

    private DepthStencilView _depthStencilView;

    private SharpDX.Color _background = SharpDX.Color.White;

    private static Renderer _instance;

    private Renderer(IntPtr ptr) {
      _formPtr = ptr;

      InitializeDeviceResources();
      InitializeShaders();
      InitializeDepthBuffer();
    }

    public static Renderer GetRenderer(IntPtr ptr) {
      if (_instance == null) {
        _instance = new Renderer(ptr);
      }
      return _instance;
    }

    public static Renderer GetRenderer() {
      return _instance;
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InitializeDeviceResources() {
      var description = new SwapChainDescription {
        BufferCount = 1,
        ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
        IsWindowed = true,
        OutputHandle = _formPtr,
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
        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0, InputClassification.PerVertexData, 0),
        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 24, 0, InputClassification.PerVertexData, 0),
      });

      _context3D.InputAssembler.InputLayout = _inputLayout;
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Update() {
      _context3D.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);
      _context3D.ClearRenderTargetView(_renderTargetView, _background);
      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetLightConstantBuffer(PsLightConstantBuffer[] amLightData) {
      for (int i = 0; i < amLightData.Length; ++i) {
        _constantLightBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref amLightData[i]);
        _context3D.PixelShader.SetConstantBuffer(i, _constantLightBuffer);
      }
    }

    public void SetVerticesBuffer(VsBuffer[] vertices) {
      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, vertices);
      _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<VsBuffer>(), 0));
    }

    public void SetIndicesBuffer(short[] indices) {
      _indexBuffer = Buffer.Create(_device3D, BindFlags.IndexBuffer, indices);
      _context3D.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);
    }

    public void SetMvpConstantBuffer(VsMvpConstantBuffer matrices) {
      _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref matrices);
      _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);
    }

    public void Draw(int count) {
      _context3D.DrawIndexed(count, 0, 0);
    }

    public void Present() {
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
      _depthStencilView.Dispose();

    }

    public void InitializeDepthBuffer() {

      var depthStencilDesc = new DepthStencilStateDescription {
        IsDepthEnabled = true,
        DepthWriteMask = DepthWriteMask.All,
        DepthComparison = Comparison.Less,
        IsStencilEnabled = false
      };

      var depthStencilState = new DepthStencilState(_device3D, depthStencilDesc);
      _context3D.OutputMerger.SetDepthStencilState(depthStencilState);

      var depthBufferDesc = new Texture2DDescription {
        Width = Width,
        Height = Height,
        ArraySize = 1,
        MipLevels = 1,
        Format = Format.D24_UNorm_S8_UInt,
        SampleDescription = new SampleDescription(1, 0),
        Usage = ResourceUsage.Default,
        BindFlags = BindFlags.DepthStencil,
        CpuAccessFlags = CpuAccessFlags.None,
        OptionFlags = ResourceOptionFlags.None
      };

      var depthBuffer = new Texture2D(_device3D, depthBufferDesc);

      var depthStencilViewDesc = new DepthStencilViewDescription {
        Format = depthBufferDesc.Format,
        Dimension = DepthStencilViewDimension.Texture2D
      };

      _depthStencilView = new DepthStencilView(_device3D, depthBuffer, depthStencilViewDesc);
      _context3D.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
    }
  }
}
