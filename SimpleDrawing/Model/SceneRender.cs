using ImGuiNET;
using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Model {

  internal class SceneRender : IDisposable {

    private readonly Window _window;

    private SceneDrawer _drawer;
    private int _fbo;
    private int _rbo;
    private int _texColor;
    private OpenTK.Mathematics.Vector2i _fboSize = default;

    public SceneRender(Window _window) {
      _drawer = new();
      _drawer.OnLoad();

      this._window = _window;
    }

    public void Dispose() {
      _drawer.OnClosed();
      GL.DeleteFramebuffer(_fbo);
    }



    public void ChangeShowingType(int i, bool state) {
      _drawer.ChangeDrawingType(i, state);
    }

    public void DrawViewportWindow() {
      Error.Check();

      ImGui.Begin("GameWindow");

      {
        ImGui.BeginChild("GameRender");

        System.Numerics.Vector2 wsize = ImGui.GetWindowSize();

        OpenTK.Mathematics.Vector2i wsizei = new((int)wsize.X, (int)wsize.Y);
        if (_fboSize != wsizei) {
          _fboSize = wsizei;

          if (_fbo == 0) {
            _fbo = GL.GenFramebuffer();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
            GL.ObjectLabel(ObjectLabelIdentifier.Framebuffer, _fbo, 10, "GameWindow");
          }

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

          FramebufferErrorCode errorCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
          if (errorCode != FramebufferErrorCode.FramebufferComplete)
            throw new Exception();
        } else {
          GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
          GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rbo);
        }

        Error.Check();
        GL.Viewport(0, 0, wsizei.X, wsizei.Y);

        {
          GL.ClearColor(OpenTK.Mathematics.Color4.DimGray);
          GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

          _drawer.OnResize(wsizei.X, wsizei.Y);
          _drawer.OnRenderFrame();
          Error.Check();
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        GL.Viewport(0, 0, _window.ClientSize.X, _window.ClientSize.Y);

        Error.Check();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _texColor);

        ImGui.Image(new IntPtr(_texColor), wsize);

        Error.Check();
        ImGui.EndChild();
      }

      ImGui.End();
    }

    public void SetEdgesColor(System.Numerics.Vector3 color) {
      _drawer.ChangeEdgesColor(new OpenTK.Mathematics.Vector3(color.X, color.Y, color.Z));
    }

    public void SetPointsColor(System.Numerics.Vector3 color) {
      _drawer.ChangePointsColor(new OpenTK.Mathematics.Vector3(color.X, color.Y, color.Z));
    }

    public void MoveCameraFwd(float val) { _drawer.MoveCameraFwd(val); }

    public void MoveCameraBack(float val) { _drawer.MoveCameraBack(val); }

    public void MoveCameraRight(float val) { _drawer.MoveCameraRight(val); }

    public void MoveCameraLeft(float val) { _drawer.MoveCameraLeft(val); }

    public void MoveCameraDown(float val) { _drawer.MoveCameraDown(val); }

    public void MoveCameraUp(float val) { _drawer.MoveCameraUp(val); }

    public void ChangeCameraPitch(float val) { _drawer.ChangeCameraPitch(val); }

    public void ChangeCameraYaw(float val) { _drawer.ChangeCameraYaw(val); }

    public void ChangeDirectionalLight(DirectionalLight light) { _drawer.ChangeDirLight(light); }

    public void ChangePointLight(PointLight light) { _drawer.ChangePointLight(light); }

    public void ChangeFlashLight(FlashLight light) { _drawer.ChangeFlashLight(light); }

    public void SetTime(string time) {
      _drawer.SetTime(time);
    }

    public void ReGenerateVolumes(int count, float step) {
      _drawer.ReplaceVolumes(count, step);
    }

    public void RegenerateFlashLights(int count, float step) {

    }

    public void ChangeMovements(int mode, bool state) {
      _drawer.ChangeMovingActions(mode, state);
    }

  }
}

