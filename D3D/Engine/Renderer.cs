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
    private DepthStencilView _depthStencilView;

    private long _indicesCount;
    private long _verticesCount;

    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;
    private Buffer[] _constantLightBuffers;
    private Buffer _constantBuffer;

    private SharpDX.Color _background = SharpDX.Color.White;

    private static Renderer _instance;

    private Renderer(IntPtr ptr) {
      _formPtr = ptr;
      _verticesCount = 0;
      _indicesCount = 0;

      InitializeDeviceResources();
      SetViewPort();
      InputElement[] element = new[] {
        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0, InputClassification.PerVertexData, 0),
        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 24, 0, InputClassification.PerVertexData, 0),
        new InputElement("COLOR", 0, Format.R32G32B32_Float, 32, 0, InputClassification.PerVertexData, 0),
        new InputElement("GRIDCOORDS", 0, Format.R32G32B32_SInt, 44, 0, InputClassification.PerVertexData, 0)
      };
      InitializeVertexShader("Shaders/VertexShader.hlsl", ref element);
      InitializePixelShader("Shaders/PixelShader.hlsl");
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

      Device.CreateWithSwapChain(DriverType.Hardware, 
                                 DeviceCreationFlags.None, 
                                 description, 
                                 out _device3D, 
                                 out _swapChain);
      _context3D = _device3D.ImmediateContext;

      using (Texture2D backBuffer = _swapChain.GetBackBuffer<Texture2D>(0)) {
        _renderTargetView = new RenderTargetView(_device3D, backBuffer);
      }
    }

    public void SetViewPort() {
      var viewport = new Viewport(0, 0, Width, Height);
      _context3D.Rasterizer.SetViewport(viewport);
    }

    public void InitializeVertexShader(string path, ref InputElement[] elem) {

      using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_4_0", ShaderFlags.Debug)) {
        var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
        var vShader = new VertexShader(_device3D, vertexShaderByteCode);
        _context3D.VertexShader.Set(vShader);
        _context3D.InputAssembler.InputLayout = new InputLayout(_device3D, signature, elem);
      }

    }

    public void InitializePixelShader(string path) {
      using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_4_0", ShaderFlags.Debug)) {
        var pShader = new PixelShader(_device3D, pixelShaderByteCode);
        _context3D.PixelShader.Set(pShader);
      }
    }

    public void ChangePrimitiveTopology(PrimitiveTopology type) {
      _context3D.InputAssembler.PrimitiveTopology = type;
    }

    public void Update() {
      _context3D.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);
      _context3D.ClearRenderTargetView(_renderTargetView, _background);
    }

    
    public void SetLightConstantBuffer(ref PsLightConstantBuffer[] amLightData) {
      if (_constantLightBuffers == null || _constantLightBuffers.Length != amLightData.Length) {
        for (int i = 0; i < _constantLightBuffers?.Length; i++) {
          _constantLightBuffers[i]?.Dispose();
        }
        _constantLightBuffers = new Buffer[amLightData.Length];
      }

      for (int i = 0; i < amLightData.Length; i++) {
        if (_constantLightBuffers[i] == null) {
          _constantLightBuffers[i] = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref amLightData[i]);
        } else {
          _context3D.UpdateSubresource(ref amLightData[i], _constantLightBuffers[i]);
        }
        _context3D.PixelShader.SetConstantBuffer(i, _constantLightBuffers[i]);
      }
    }

    public void SetVerticesBuffer(ref VsBuffer[] vertices) {
      if (_vertexBuffer == null || _verticesCount != vertices.Length) {
        _vertexBuffer?.Dispose();

        _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, vertices);
        _verticesCount = vertices.Length;
        _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<VsBuffer>(), 0));
      } else {
        _context3D.UpdateSubresource(vertices, _vertexBuffer);
      }
    }


    public void SetIndicesBuffer(ref short[] indices) {
      if (_indexBuffer == null || _indicesCount != indices.Length) {
        _indexBuffer?.Dispose();

        _indexBuffer = Buffer.Create(_device3D, BindFlags.IndexBuffer, indices);
        _indicesCount = indices.Length;
        _context3D.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);
      } else {
        _context3D.UpdateSubresource(indices, _indexBuffer);
      }
    }

    public void SetMvpConstantBuffer(ref VsMvpConstantBuffer matrices) {
      if (_constantBuffer == null) {
        _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref matrices);
        _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);
      } else {
        _context3D.UpdateSubresource(ref matrices, _constantBuffer);
      }
    }

    public void Draw(int count) {
      _context3D.DrawIndexed(count, 0, 0);
    }

    public void Present() {
      _swapChain.Present(1, PresentFlags.None);
    }


    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Dispose() {
      _renderTargetView.Dispose();
      _swapChain.Dispose();
      _device3D.Dispose();
      _context3D.Dispose();
      _depthStencilView.Dispose();
    }

    private void InitializeDepthBuffer() {

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
