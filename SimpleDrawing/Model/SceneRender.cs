using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

    public void MoveCameraFwd(float val) { _drawer.MoveCameraFwd(val); }

    public void MoveCameraBack(float val) { _drawer.MoveCameraBack(val); }

    public void MoveCameraRight(float val) { _drawer.MoveCameraRight(val); }

    public void MoveCameraLeft(float val) { _drawer.MoveCameraLeft(val); }

    public void MoveCameraDown(float val) { _drawer.MoveCameraDown(val); }

    public void MoveCameraUp(float val) { _drawer.MoveCameraUp(val); }

    public void ChangeCameraPitch(float val) { _drawer.ChangeCameraPitch(val); }

    public void ChangeCameraYaw(float val) { _drawer.ChangeCameraYaw(val); }
    public void ChangeDirLightDirection(Vector3 dir) { _drawer.ChangeDirLightDirection(dir); }
    public void ChangeDirLightColor(Vector3 dir) { _drawer.ChangeDirLightColor(dir); }
    public void ChangeDirLightDiffuse(Vector3 dir) { _drawer.ChangeDirLightDiffuse(dir); }
    public void ChangeDirLightSpecular(Vector3 dir) { _drawer.ChangeDirLightSpecular(dir); }


    public void ChangePointLightColor(Vector3 dir) { _drawer.ChangePointLightColor(dir); }
    public void ChangePointLightDiffuse(Vector3 dir) { _drawer.ChangePointLightDiffuse(dir); }
    public void ChangePointLightSpecular(Vector3 dir) { _drawer.ChangePointLightSpecular(dir); }
    public void ChangePointLightConstant(float dir) { _drawer.ChangePointLightConstant(dir); }
    public void ChangePointLightLinear(float dir) { _drawer.ChangePointLightLinear(dir); }
    public void ChangePointLightQuadratic(float dir) { _drawer.ChangePointLightQuadratic(dir); }
    public void ChangeFlashLightColor(Vector3 dir) { _drawer.ChangeFlashLightColor(dir); }
    public void ChangeFlashLightDirection(Vector3 val) {  _drawer.ChangeFlashLightDirection(val); }
    public void ChangeFlashLightDiffuse(Vector3 val) { _drawer.ChangeFlashLightDiffuse(val); }
    public void ChangeFlashLightSpecular(Vector3 val) { _drawer.ChangeFlashLightSpecular(val); }
    public void ChangeFlashLightConstant(float val) { _drawer.ChangeFlashLightConstant(val); }
    public void ChangeFlashLightLinear(float val) { _drawer.ChangeFlashLightLinear(val); }
    public void ChangeFlashLightQuadratic(float val) { _drawer.ChangeFlashLightQuadratic(val); }

    public void SetTime(string time) {
      _drawer.SetTime(time);
    }

    public void ReGenerateVolumes(int count, float step) {
      _drawer.ReplaceVolumes(count, step);
    }

    public void RegenerateFlashLights(int count, float step) {

    }

    public void ChangeMovements(int mode, bool state) {
      _drawer.ChangeCubesMovings(mode, state);
    }

    public void ChangeLightsMovements(int mode, bool state) {
      _drawer.ChangeLightsMovings(mode, state);
    }

  }
}

