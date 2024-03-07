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


struct Vertex {
  public Vector3 _pos;
  public Color3 _color;
}

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

  private VertexShader _vertexShader;
  private PixelShader _pixelShader;
  private Buffer _vertexBuffer;
  private Buffer _indexBuffer;

  Vertex[] _vertices;

  // test
  private SharpDX.Color _background = SharpDX.Color.White;
  // test

  public MyForm() {
    _vertices = new Vertex[3];
    _vertices[0] = new Vertex { _pos = new Vector3(-0.5f, 0.0f, 0.0f), _color = new Color3(0.0f, 0.0f, 1.0f) };
    _vertices[1] = new Vertex { _pos = new Vector3(0.5f, 0.0f, 0.0f), _color = new Color3(1.0f, 0.0f, 0.0f) };
    _vertices[2] = new Vertex { _pos = new Vector3(0.0f, 0.5f, 0.0f), _color = new Color3(0.0f, 1.0f, 0.0f) };

    _renderForm = new RenderForm();
    _renderForm.ClientSize = new Size(Width, Height);

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
  private void InitializeDeviceResources() {
    var description = new SwapChainDescription {
      BufferCount = 1,
      ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1),
      Format.R8G8B8A8_UNorm),
      IsWindowed = true,
      OutputHandle = _renderForm.Handle,
      SampleDescription = new SampleDescription(1, 0),
      SwapEffect = SwapEffect.Discard,
      Usage = Usage.RenderTargetOutput
    };

    Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out _device3D, out _swapChain);
    _context3D = _device3D.ImmediateContext;

    using (var resource = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0)) {
      _renderTargetView = new RenderTargetView(_device3D, resource);

    }

    _context3D.Rasterizer.SetViewport(0, 0, Width, Height);

  }
  // /////////////////////////////////////////////////////////////////////////////////////////////////////////

  private void InitializeBuffers() {

    BufferDescription bufferDescription = new BufferDescription() {
      BindFlags = BindFlags.VertexBuffer,
      CpuAccessFlags = CpuAccessFlags.Write,
      OptionFlags = ResourceOptionFlags.None,
      SizeInBytes = Utilities.SizeOf<Vertex>() * _vertices.Length,
      Usage = ResourceUsage.Dynamic,
    };

    _vertexBuffer = Buffer.Create(_device3D, _vertices, bufferDescription);

    DataStream mappedResource;
    _context3D.MapSubresource(_vertexBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

    mappedResource.WriteRange(_vertices);
    _context3D.UnmapSubresource(_vertexBuffer, 0);



  }

  // /////////////////////////////////////////////////////////////////////////////////////////////////////////

  private void InitializeShaders() {

    using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile("shaders.shader", "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None)) {
      _vertexShader = new VertexShader(_device3D, vertexShaderByteCode);

      InputElement[] inputElements = new InputElement[] {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0, 12),
      };

      InputLayout inputLayout = new InputLayout(_device3D, vertexShaderByteCode, inputElements);
      _context3D.InputAssembler.InputLayout = inputLayout;
    }

    using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile("shaders.shader", "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None))
      _pixelShader = new PixelShader(_device3D, pixelShaderByteCode);

    _context3D.VertexShader.Set(_vertexShader);
    _context3D.PixelShader.Set(_pixelShader);

  }


  public void Run() {
    RenderLoop.Run(_renderForm, RenderCallback);
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

    // my code
    int stride = Utilities.SizeOf<Vertex>();
    int offset = 0;
    _context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, stride, offset));
    _context3D.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
    _context3D.Draw(3, 0);


    _swapChain.Present(0, PresentFlags.None);

  }
  // /////////////////////////////////////////////////////////////////////////////////////////////////////////

  public void Dispose() {
    _context3D.ClearState();
    _context3D.Flush();
    _context3D.Dispose();
    _device3D.Dispose();
    _swapChain.Dispose();
    _renderForm.Dispose();
  }
  // /////////////////////////////////////////////////////////////////////////////////////////////////////////

}







/*
using SharpDX.Windows;
using System.Drawing;
using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using Vector3 = SharpDX.Vector3;
using SharpDX.D3DCompiler;
using System.Collections.Generic;
using System.IO;

class MyRenderForm : IDisposable {
  private const int Width = 800;
  private const int Height = 600;

  private RenderForm _renderForm;

  private Device _device3D;
  private SwapChain _swapChain;
  private DeviceContext _context3D;


  private Buffer _vertexBuffer;
  private Buffer _indexBuffer;
  private VertexShader _vertexShader;
  private PixelShader _pixelShader;
  private InputLayout _inputLayout;
  private RenderTargetView _renderTargetView;

  private Vector3[] _vertices;
  private ushort[] _indices;

  private Button _myButton;

  public MyRenderForm() {
    _renderForm = new RenderForm("SharpDX - MiniCube Direct3D11 Sample");
    _renderForm.ClientSize = new Size(Width, Height);
    _renderForm.Location = new System.Drawing.Point(0, 0);

    InitializeDeviceResources();

    /*
    InitializeShaders(); 
    InitializeBuffers();




  private void RenderCallback() {
    _context3D.ClearRenderTargetView(_renderTargetView, SharpDX.Color.LightSteelBlue);
    _context3D.Rasterizer.SetViewport(0, 0, Width, Height);


    _swapChain.Present(0, PresentFlags.None);
  }

  public void Dispose() {
    _context3D.ClearState();
    _context3D.Flush();
    _context3D.Dispose();
    _device3D.Dispose();
    _swapChain.Dispose();
    _renderForm.Dispose();


    _vertexBuffer.Dispose();
    _indexBuffer.Dispose();
    _inputLayout.Dispose();
    _vertexShader.Dispose();
    _pixelShader.Dispose();
  }

  private void InitializeBuffers() {
    _vertices = new[]   {
        new Vector3(-1.0f, 1.0f, -1.0f),
        new Vector3(1.0f, 1.0f, -1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(-1.0f, 1.0f, 1.0f),

        new Vector3(-1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f)
      };

    _indices = new ushort[]   {
        3, 1, 0,
        2, 1, 3,

        0, 5, 4,
        1, 5, 0,

        3, 4, 7,
        0, 4, 3,

        1, 6, 5,
        2, 6, 1,

        2, 7, 6,
        3, 7, 6,

        2, 3, 7,
        2, 0, 3,

        5, 6, 7,
        4, 5, 7
      };

      _vertexBuffer = Buffer.Create(_device3D, BindFlags.VertexBuffer, _vertices); 
      _indexBuffer = Buffer.Create(_device3D, BindFlags.IndexBuffer, _indices);
    }

    private void InitializeShaders() {
    using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile("VertexShader.hlsl", "main", "vs_4_0", ShaderFlags.None, EffectFlags.None)) {
      _vertexShader = new VertexShader(_device3D, vertexShaderByteCode);
      _inputLayout = new InputLayout(_device3D, ShaderSignature.GetInputSignature(vertexShaderByteCode), new[] {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) });
    }

    using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile("PixelShader.hlsl", "main", "ps_4_0", ShaderFlags.None, EffectFlags.None))
      _pixelShader = new PixelShader(_device3D, pixelShaderByteCode);
  }


}


_context3D.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf(), 0)); 
_context3D.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0); 
_context3D.InputAssembler.InputLayout = _inputLayout;

_context3D.VertexShader.Set(_vertexShader); _context3D.PixelShader.Set(_pixelShader);

_context3D.DrawIndexed(36, 0, 0); _swapChain.Present(0, PresentFlags.None);
*/


