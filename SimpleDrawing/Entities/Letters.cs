using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Entities {
  public class Letters {
    public float Width { get; set; }
    public float Height { get; set; }
    public float CoordX { get; set; }
    public float CoordY { get; set; }
    public float Scale { get; set; }

    private List<Texture> _textures;

    private float[] _vertices;
    private float[] _texCoords;
    private int _vao;


    public Letters(float width, float height) {

      Width = width;
      Height = height;
      CoordX = -1.0f + width + 0.05f;
      CoordY = -1.0f + height + 0.05f;
      Scale = 1.5f;

      _textures = new List<Texture>();
      _vertices = new float[] { 0.0f, Height, Width, Height, 0.0f, 0.0f, 0.0f, 0.0f, Width, Height, Width, 0.0f };
      _texCoords = new float[] { 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f };

      _textures.Add(Texture.LoadFromFile("Resources/0.png"));
      _textures.Add(Texture.LoadFromFile("Resources/1.png"));
      _textures.Add(Texture.LoadFromFile("Resources/2.png"));
      _textures.Add(Texture.LoadFromFile("Resources/3.png"));
      _textures.Add(Texture.LoadFromFile("Resources/4.png"));
      _textures.Add(Texture.LoadFromFile("Resources/5.png"));
      _textures.Add(Texture.LoadFromFile("Resources/6.png"));
      _textures.Add(Texture.LoadFromFile("Resources/7.png"));
      _textures.Add(Texture.LoadFromFile("Resources/8.png"));
      _textures.Add(Texture.LoadFromFile("Resources/9.png"));
      _textures.Add(Texture.LoadFromFile("Resources/..png"));
      _textures.Add(Texture.LoadFromFile("Resources/F.png"));
      _textures.Add(Texture.LoadFromFile("Resources/P.png"));
      _textures.Add(Texture.LoadFromFile("Resources/S.png"));

      _vao = GL.GenVertexArray();

    }

    public void DrawFps(ref Shader shader, string fps) {

      BindTextureBuffer(shader.Handle);

      for (int i = 0; i < 3; ++i) {

        OpenTK.Mathematics.Vector3 position = new OpenTK.Mathematics.Vector3(CoordX, CoordY, 0);

        OpenTK.Mathematics.Matrix4 modelMatrix = OpenTK.Mathematics.Matrix4.CreateScale(1.5f)
            * OpenTK.Mathematics.Matrix4.CreateTranslation(position);

        shader.SetMatrix4("modelMatrix", modelMatrix);

        _texture = _letters.Textures[i];
        _texture.Use(TextureUnit.Texture0);

        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 2);
        x += 0.1f;
      }




    }

    private void BindTextureBuffer(int shaderHandle) {

      GL.BindVertexArray(_vao);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

      GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
      int vertexLocation = GL.GetAttribLocation(shaderHandle, "aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

      GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
      GL.BufferData(BufferTarget.ArrayBuffer, _texCoords.Length * sizeof(float), _texCoords, BufferUsageHint.StaticDraw);
      int textureLocation = GL.GetAttribLocation(shaderHandle, "aTexCoord");
      GL.EnableVertexAttribArray(textureLocation);
      GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

    }

  }
}
