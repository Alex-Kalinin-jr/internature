using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SimpleDrawing.Entities;

namespace SimpleDrawing;

internal class SceneRender : IDisposable {

  private SceneDrawer _drawer;

  int _fbo;
  int _rbo;
  int _texColor;
  Vector2i _fboSize = default;
  private readonly Window _window;

  public SceneRender(Window _window) {
    _drawer = new();
    _drawer.OnLoad();

    this._window = _window;
  }

  public void ChangeShowingType(int i, bool state) {
    _drawer.ChangeDrawingType(i, state);
  }

  public void DrawViewportWindow() {
    Error.Check();
    // https://gamedev.stackexchange.com/a/140704

    ImGui.Begin("GameWindow");
    {
      // Using a Child allow to fill all the space of the _window.
      // It also alows customization
      ImGui.BeginChild("GameRender");

      // Get the size of the child (i.e. the whole draw size of the windows).
      System.Numerics.Vector2 wsize = ImGui.GetWindowSize();

      // make sure the buffers are the currect size
      Vector2i wsizei = new((int)wsize.X, (int)wsize.Y);
      if (_fboSize != wsizei) {
        _fboSize = wsizei;

        // create our frame buffer if needed
        if (_fbo == 0) {
          _fbo = GL.GenFramebuffer();
          // bind our frame buffer
          GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
          GL.ObjectLabel(ObjectLabelIdentifier.Framebuffer, _fbo, 10, "GameWindow");
        }

        // bind our frame buffer
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

        if (_texColor > 0)
          GL.DeleteTexture(_texColor);

        _texColor = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _texColor);
        GL.ObjectLabel(ObjectLabelIdentifier.Texture, _texColor, 16, "GameWindow:Color");
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, wsizei.X, wsizei.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _texColor, 0);

        if (_rbo > 0)
          GL.DeleteRenderbuffer(_rbo);

        _rbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
        GL.ObjectLabel(ObjectLabelIdentifier.Renderbuffer, _rbo, 16, "GameWindow:Depth");
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32f, wsizei.X, wsizei.Y);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, _rbo);
        //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        //texDepth = GL.GenTexture();
        //GL.BindTexture(TextureTarget.Texture2D, texDepth);
        //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, 800, 600, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
        //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texDepth, 0);

        // make sure the frame buffer is complete
        FramebufferErrorCode errorCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (errorCode != FramebufferErrorCode.FramebufferComplete)
          throw new Exception();
      } else {
        // bind our frame and depth buffer
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
      }

      Error.Check();
      GL.Viewport(0, 0, wsizei.X, wsizei.Y); // change the viewport to _window

      // actually draw the scene
      {
        GL.ClearColor(Color4.DimGray);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _drawer.OnResize(wsizei.X, wsizei.Y);
        _drawer.OnRenderFrame();
        Error.Check();
      }

      // unbind our bo so nothing else uses it
      GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

      GL.Viewport(0, 0, _window.ClientSize.X, _window.ClientSize.Y); // back to full screen size

      Error.Check();
      // Because I use the texture from OpenGL, I need to invert the V from the UV.
      GL.ActiveTexture(TextureUnit.Texture0);
      GL.BindTexture(TextureTarget.Texture2D, _texColor);
      //ImGui.Image(new IntPtr(_texColor), wsize, Vector2.UnitY, Vector2.UnitX);
      ImGui.Image(new IntPtr(_texColor), wsize);

      Error.Check();
      ImGui.EndChild();
    }
    ImGui.End();
  }

  public void Dispose() {
    _drawer.OnClosed();
    GL.DeleteFramebuffer(_fbo);
  }


  // ///////////////////////////////////////////////////////////////////
  public void SetEdgesColor(System.Numerics.Vector3 color) {
    _drawer.ChangeEdgesColor(new Vector3(color.X, color.Y, color.Z));
  }

  public void SetPointsColor(System.Numerics.Vector3 color) {
    _drawer.ChangePointsColor(new Vector3(color.X, color.Y, color.Z));
  }

  public void MoveCameraFwd(float val) { _drawer.MoveCameraFwd(val); }

  public void MoveCameraBack(float val) { _drawer.MoveCameraBack(val); }

  public void MoveCameraRight(float val) { _drawer.MoveCameraRight(val); }

  public void MoveCameraLeft(float val) { _drawer.MoveCameraLeft(val); }

  public void MoveCameraDown(float val) { _drawer.MoveCameraDown(val); }

  public void MoveCameraUp(float val) { _drawer.MoveCameraUp(val); }

  public void ChangeCameraPitch(float val) { _drawer.ChangeCameraPitch(val); }

  public void ChangeCameraYaw(float val) { _drawer.ChangeCameraYaw(val); }

  internal void ChangeDirectionalLight(DirectionalLight light) { _drawer.ChangeDirLight(light); }

  internal void ChangePointLight(PointLight light) { _drawer.ChangePointLight(light); }

  internal void ChangeFlashLight(FlashLight light) { _drawer.ChangeFlashLight(light); }

}
