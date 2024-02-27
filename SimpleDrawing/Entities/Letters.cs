using OpenTK.Graphics.OpenGL4;

namespace SimpleDrawing.Entities {
  public class Letters {
    public float Width { get; set; }
    public float Height { get; set; }
    public float CoordX { get; set; }
    public float CoordY { get; set; }
    public float Scale { get; set; }

    private Dictionary<char, Texture> _texturesByName;

    private float[] _vertices;
    private float[] _texCoords;
    private int _vao;


    public Letters(float width, float height) {

      Width = width;
      Height = height;
      CoordX = -1.0f + width + 0.05f;
      CoordY = -1.0f + height + 0.05f;
      Scale = 1.5f;

      _texturesByName = new Dictionary<char, Texture>();
      _vertices = new float[] { 0.0f, Height, Width, Height, 0.0f, 0.0f, 0.0f, 0.0f, Width, Height, Width, 0.0f };
      _texCoords = new float[] { 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f };

      _texturesByName.Add('0',Texture.LoadFromFile("Resources/0.png"));
      _texturesByName.Add('1', Texture.LoadFromFile("Resources/1.png"));
      _texturesByName.Add('2', Texture.LoadFromFile("Resources/2.png"));
      _texturesByName.Add('3', Texture.LoadFromFile("Resources/3.png"));
      _texturesByName.Add('4', Texture.LoadFromFile("Resources/4.png"));
      _texturesByName.Add('5', Texture.LoadFromFile("Resources/5.png"));
      _texturesByName.Add('6', Texture.LoadFromFile("Resources/6.png"));
      _texturesByName.Add('7', Texture.LoadFromFile("Resources/7.png"));
      _texturesByName.Add('8', Texture.LoadFromFile("Resources/8.png"));
      _texturesByName.Add('9', Texture.LoadFromFile("Resources/9.png"));
      _texturesByName.Add('.', Texture.LoadFromFile("Resources/..png"));
      _texturesByName.Add('F', Texture.LoadFromFile("Resources/F.png"));
      _texturesByName.Add('P', Texture.LoadFromFile("Resources/P.png"));
      _texturesByName.Add('S', Texture.LoadFromFile("Resources/S.png"));

      _vao = GL.GenVertexArray();

    }

    public void DrawFps(ref Shader shader, string fps) {

      BindTextureBuffer(shader.Handle);

      float x = CoordX;

      foreach (char symbol in fps) {

        if (_texturesByName.ContainsKey(symbol)) {
          OpenTK.Mathematics.Vector3 position = new OpenTK.Mathematics.Vector3(x, CoordY, 0);

          OpenTK.Mathematics.Matrix4 modelMatrix = OpenTK.Mathematics.Matrix4.CreateScale(Scale)
              * OpenTK.Mathematics.Matrix4.CreateTranslation(position);

          shader.SetMatrix4("modelMatrix", modelMatrix);


          _texturesByName[symbol].Use(TextureUnit.Texture0);

          GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 2);
          x += Width * Scale;
        }

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
