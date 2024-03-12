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

  public class Renderer : IDisposable {
    private const int Width = 800;
    private const int Height = 600;

    public Camera _camera;
    private IntPtr _formPtr;

    private Device _device3D;
    private SwapChain _swapChain;
    private DeviceContext _context3D;
    private RenderTargetView _renderTargetView;

    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;
    private Buffer _constantBuffer;

    private VertexShader _vertexShader;
    private PixelShader _pixelShader;

    private SharpDX.Color _background = SharpDX.Color.White;

    private ShaderSignature _inputSignature;
    private InputLayout _inputLayout;

    public Renderer(IntPtr ptr) {

      _formPtr = ptr;
      _camera = new Camera();

      InitializeDeviceResources();
      InitializeShaders();

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
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, InputClassification.PerVertexData, 0)
      });
      _context3D.InputAssembler.InputLayout = _inputLayout;

      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;


    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RenderCallback(Vertex[] vertices, short[] indices) {
      _camera.Update();
      var tmp = new VS_CONSTANT_BUFFER();
      tmp.view = _camera.View;
      tmp.view.Transpose();

      tmp.projection = _camera.Projection;
      tmp.projection.Transpose();

      tmp.world = ComputeModelMatrix();
      tmp.world.Transpose();

      _constantBuffer = Buffer.Create(_device3D, BindFlags.ConstantBuffer, ref tmp);
      _context3D.VertexShader.SetConstantBuffer(0, _constantBuffer);

      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, vertices);
      _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));

      _indexBuffer = Buffer.Create(_device3D, BindFlags.IndexBuffer, indices);
      _context3D.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);

      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);
      _context3D.ClearRenderTargetView(_renderTargetView, _background);

      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

      _context3D.DrawIndexed(indices.Length, 0, 0);

      _swapChain.Present(1, PresentFlags.None);

    }

    // mistake
    public Matrix ComputeModelMatrix() {
      var buff =
        Matrix.RotationYawPitchRoll(0.0f, 0.0f, 0.0f);
      Matrix.Translation(0.0f, 0.0f, 0.0f);
      buff.Transpose();
      return buff;
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

    }

    public void ChangePitch(float pitch) {
      _camera.RotateX(pitch);
    }

    public void ChangeYaw(float yaw) { 
      _camera.RotateY(yaw);
    }

    public void MoveCameraUpDown(float val) {

      Vector3 buff = _camera.Position;
      buff.Y += val;
      _camera.Position = buff;

      buff = _camera.Target;
      buff.Y += val;
      _camera.Target = buff;
    }

    public void MoveCameraLeftRight(float val) {

      Vector3 buff = _camera.Position;
      buff.X += val;
      _camera.Position = buff;

      buff = _camera.Target;
      buff.X += val;
      _camera.Target = buff;
    }

  }
}
