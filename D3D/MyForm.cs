using System;
using System.Drawing;
using SharpDX.Windows;

namespace D3D {

  public class MyForm : IDisposable {

    private const int Width = 800;
    private const int Height = 600;

    private RenderForm _renderForm;
    private Renderer _renderer;

    public MyForm() {

      _renderForm = new RenderForm();
      _renderForm.ClientSize = new Size(Width, Height);
      _renderForm.AllowUserResizing = false;

      _renderer = new Renderer(_renderForm.Handle);

    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Run() {
      RenderLoop.Run(_renderForm, RenderCallback);
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RenderCallback() {
      _renderer.RenderCallback();
    }

    // /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Dispose() {

      _renderForm.Dispose();
      _renderer.Dispose();

    }

  }
}
