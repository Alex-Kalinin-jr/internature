using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using SharpDX.D3DCompiler;
using System.Drawing;

namespace D3D {

  /// <summary>
  /// Class responsible for rendering graphics using DirectX 11.
  /// </summary>
  public class Renderer : IDisposable {

    private const int Width = 1024; // just an initial value
    private const int Height = 768; // just an initial value

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
    private Buffer _constantSliceBuffer;
    private RasterizerState _rasterizerState;
    private Texture2D _depthBuffer;

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
        new InputElement("COLOR", 0, Format.R32G32B32_Float, 12, 0, InputClassification.PerVertexData, 0),
        new InputElement("GRIDCOORDS", 0, Format.R32G32B32_SInt, 24, 0, InputClassification.PerVertexData, 0)
      };
      InitializeVertexShader("Engine/Shaders/VertexShader.hlsl", ref element);
      InitializePixelShader("Engine/Shaders/PixelShader.hlsl");
      ChangeRasterizerState(FillMode.Solid);
      ChangeBlendState();
    }

    /// <summary>
    /// Gets the instance of the Renderer class.
    /// </summary>
    /// <param name="ptr">The pointer to the window's form.</param>
    /// <returns>The instance of the Renderer class.</returns>
    public static Renderer GetRenderer(IntPtr ptr) {
      if (_instance == null) {
        _instance = new Renderer(ptr);
      }
      return _instance;
    }

    /// <summary>
    /// Gets the instance of the Renderer class.
    /// </summary>
    /// <returns>The instance of the Renderer class.</returns>
    public static Renderer GetRenderer() {
      return _instance;
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Initializes the device resources for rendering.
    /// </summary>
    private void InitializeDeviceResources() {

      // Define the buffer description
      var bufferDesc = new ModeDescription {
        Width = Width,
        Height = Height,
        RefreshRate = new Rational(60, 1),
        Format = Format.R8G8B8A8_UNorm,
        ScanlineOrdering = DisplayModeScanlineOrder.Unspecified,
        Scaling = DisplayModeScaling.Unspecified
      };

      var swapChainDesc = new SwapChainDescription {
        BufferCount = 1,
        OutputHandle = _formPtr,
        SampleDescription = new SampleDescription(1, 0),
        ModeDescription = bufferDesc,
        Usage = Usage.RenderTargetOutput,
        IsWindowed = true,
        SwapEffect = SwapEffect.Discard
      };

      Device.CreateWithSwapChain(DriverType.Hardware,
                                 DeviceCreationFlags.None,
                                 swapChainDesc,
                                 out _device3D,
                                 out _swapChain);
      _context3D = _device3D.ImmediateContext;

      Texture2D backBuffer = _swapChain.GetBackBuffer<Texture2D>(0);
      _renderTargetView = new RenderTargetView(_device3D, backBuffer);
      backBuffer.Dispose();

      // Define the depth stencil description
      var depthStencilDesc = new Texture2DDescription {
        Width = Width,
        Height = Height,
        MipLevels = 1,
        ArraySize = 1,
        Format = Format.D24_UNorm_S8_UInt,
        SampleDescription = new SampleDescription(1, 0),
        Usage = ResourceUsage.Default,
        BindFlags = BindFlags.DepthStencil,
        CpuAccessFlags = CpuAccessFlags.None,
        OptionFlags = ResourceOptionFlags.None
      };

      var depthStencilBuffer = new Texture2D(_device3D, depthStencilDesc);
      _depthStencilView = new DepthStencilView(_device3D, depthStencilBuffer);
      _context3D.OutputMerger.SetRenderTargets(_depthStencilView, _renderTargetView);
    }


    /// <summary>
    /// Sets the viewport for rendering.
    /// </summary>
    public void SetViewPort() {
      var viewport = new Viewport(0, 0, Width, Height);
      _context3D.Rasterizer.SetViewport(viewport);
    }

    /// <summary>
    /// Initializes the vertex shader for rendering.
    /// </summary>
    /// <param name="path">The file path to the vertex shader.</param>
    /// <param name="elem">The input elements for the vertex shader.</param>
    public void InitializeVertexShader(string path, ref InputElement[] elem) {

      using ( var vertexShaderByteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_4_0", ShaderFlags.Debug)) {
        var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
        var vShader = new VertexShader(_device3D, vertexShaderByteCode);
        _context3D.VertexShader.Set(vShader);
        _context3D.InputAssembler.InputLayout = new InputLayout(_device3D, signature, elem);
      }

    }

    /// <summary>
    /// Initializes the pixel shader for rendering.
    /// </summary>
    /// <param name="path">The file path to the pixel shader.</param>
    public void InitializePixelShader(string path) {
      using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_4_0", ShaderFlags.Debug)) {
        var pShader = new PixelShader(_device3D, pixelShaderByteCode);
        _context3D.PixelShader.Set(pShader);
      }
    }

    /// <summary>
    /// Changes the primitive topology for rendering.
    /// </summary>
    /// <param name="type">The primitive topology type.</param>
    public void ChangePrimitiveTopology(PrimitiveTopology type) {
      _context3D.InputAssembler.PrimitiveTopology = type;
    }

    /// <summary>
    ///
    /// <summary>
    /// Updates the renderer before drawing.
    /// </summary>
    public void Update() {
      _context3D.ClearRenderTargetView(_renderTargetView, _background);
      _context3D.ClearDepthStencilView(_depthStencilView,
                                        DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
                                        1.0f, 0);
    }

    /// <summary>
    /// Sets the constant buffer containing light data.
    /// </summary>
    /// <param name="amLightData">Array of light data.</param>
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

    /// <summary>
    /// Sets the vertex buffer.
    /// </summary>
    /// <param name="vertices">Array of vertices.</param>
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

    /// <summary>
    /// Sets the index buffer.
    /// </summary>
    /// <param name="indices">Array of indices.</param>
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

    /// <summary>
    /// Sets the model-view-projection constant buffer.
    /// </summary>
    /// <param name="matrices">The matrices for the constant buffer.</param>
    public void SetMvpConstantBuffer(ref VsMvpConstantBuffer matrices) {
      if (_constantBuffer == null) {
        _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref matrices);
        _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);
      } else {
        _context3D.UpdateSubresource(ref matrices, _constantBuffer);
      }
    }

    /// <summary>
    /// Sets the slice constant buffer.
    /// </summary>
    /// <param name="slice">The slice data for the constant buffer.</param>
    public void SetSliceConstantBuffer(ref VsSliceConstantBuffer slice) {
      if (_constantSliceBuffer == null) {
        _constantSliceBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref slice);
        _context3D.VertexShader.SetConstantBuffer(1, _constantSliceBuffer);
      } else {
        _context3D.UpdateSubresource(ref slice, _constantSliceBuffer);
      }
    }

    /// <summary>
    /// Draws the scene.
    /// </summary>
    /// <param name="count">The number of vertices to draw.</param>
    public void Draw(int count) {
      _context3D.DrawIndexed(count, 0, 0);
    }

    /// <summary>
    /// Presents the rendered scene.
    /// </summary>
    public void Present() {
      _swapChain.Present(1, PresentFlags.None);
    }

    /// <summary>
    /// Disposes the resources used by the renderer.
    /// </summary>
    public void Dispose() {
      _renderTargetView.Dispose();
      _swapChain.Dispose();
      _device3D.Dispose();
      _context3D.Dispose();
      _depthStencilView.Dispose();
      _depthBuffer.Dispose();
      _rasterizerState.Dispose();
    }

    /// <summary>
    /// Initializes the depth buffer for rendering.
    /// </summary>

    private void ChangeBlendState() {
      var blendStateDesc = new BlendStateDescription {
        AlphaToCoverageEnable = true,
        IndependentBlendEnable = false,
      };
      blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
      blendStateDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
      blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
      blendStateDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
      blendStateDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
      blendStateDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
      blendStateDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
      blendStateDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

      var blendState = new BlendState(_device3D, blendStateDesc);
      _context3D.OutputMerger.BlendState = blendState;
    }

    private void ChangeRasterizerState(FillMode mode) {
      RasterizerStateDescription rasterizerDesc = new RasterizerStateDescription {
        CullMode = CullMode.Back,
        FillMode = mode,
        IsFrontCounterClockwise = false,
        DepthBias = 0,
        DepthBiasClamp = 0,
        SlopeScaledDepthBias = 0,
        IsDepthClipEnabled = true,
        IsScissorEnabled = false,
        IsMultisampleEnabled = false,
        IsAntialiasedLineEnabled = true
      };

      _rasterizerState = new RasterizerState(_context3D.Device, rasterizerDesc);
      _context3D.Rasterizer.State = _rasterizerState;
    }
  }
}