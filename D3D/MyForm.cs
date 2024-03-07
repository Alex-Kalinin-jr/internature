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
  public class MyForm : IDisposable {
    private const int Width = 800;
    private const int Height = 600;

    private RenderForm _renderForm;
    private Button _button;
    private Button _buttonTwo;

    private Device _device3D;
    private SwapChain _swapChain;
    private DeviceContext _context3D;
    private RenderTargetView _renderTargetView;

    private Vertex[] _vertices;
    private short[] _indices;

    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;
    private VertexShader _vertexShader;
    private PixelShader _pixelShader;

    // test
    private SharpDX.Color _background = SharpDX.Color.White;
    // test

    private ShaderSignature _inputSignature;
    private InputLayout _inputLayout;

    private Viewport _viewport;

    public MyForm() {

      _renderForm = new RenderForm();
      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;

      _button = new Button();
      _button.Text = "Click me";
      _button.Location = new System.Drawing.Point(10, 10);
      _button.Click += Button_Click;
      _renderForm.Controls.Add(_button);

      _buttonTwo = new Button();
      _buttonTwo.Text = "Click me too";
      _buttonTwo.Location = new System.Drawing.Point(10, 35);
      _buttonTwo.Click += Button_Click_Two;
      _renderForm.Controls.Add(_buttonTwo);

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

      _viewport = new Viewport(0, 0, Width, Height);
      _context3D.Rasterizer.SetViewport(_viewport);

    }
    // /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void InitializeBuffers() {
      _vertices = new Vertex[] {
      new Vertex(new Vector3(-0.5f, 0.5f, 0.0f), SharpDX.Color.Red),
      new Vertex(new Vector3(0.5f, 0.5f, 0.0f), SharpDX.Color.Green),
      new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), SharpDX.Color.Blue),
    };

      _indices = new short[] { 0, 1, 2 };

      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, _vertices);
      _indexBuffer = Buffer.Create(_device3D, BindFlags.IndexBuffer, _indices);

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
      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

      _inputLayout = new InputLayout(_device3D, _inputSignature, new[] {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12)
    });

      _context3D.InputAssembler.InputLayout = _inputLayout;
      _context3D.OutputMerger.SetRenderTargets(_renderTargetView);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Button_Click(object sender, EventArgs e) {
      _button.Text = "abc";
      _background = SharpDX.Color.Aquamarine;
    }

    private void Button_Click_Two(object sender, EventArgs e) {
      _buttonTwo.Text = "abc";
      _background = SharpDX.Color.Green;
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RenderCallback() {

      _context3D.ClearRenderTargetView(_renderTargetView, _background);
      _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
      _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));
      _context3D.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);
      _context3D.DrawIndexed(_indices.Length, 0, 0);

      _swapChain.Present(0, PresentFlags.None);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Dispose() {

      _inputLayout.Dispose();
      _inputSignature.Dispose();
      _vertexBuffer.Dispose();
      _indexBuffer.Dispose();
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





