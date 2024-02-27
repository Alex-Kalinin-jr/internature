namespace SimpleDrawing.Entities {
  public class Letters {
    public List<Texture> Textures;

    public Letters() {
      Textures = new List<Texture>();

      Textures.Add(Texture.LoadFromFile("Resources/0.png"));
      Textures.Add(Texture.LoadFromFile("Resources/1.png"));
      Textures.Add(Texture.LoadFromFile("Resources/2.png"));
      Textures.Add(Texture.LoadFromFile("Resources/3.png"));
      Textures.Add(Texture.LoadFromFile("Resources/4.png"));
      Textures.Add(Texture.LoadFromFile("Resources/5.png"));
      Textures.Add(Texture.LoadFromFile("Resources/6.png"));
      Textures.Add(Texture.LoadFromFile("Resources/7.png"));
      Textures.Add(Texture.LoadFromFile("Resources/8.png"));
      Textures.Add(Texture.LoadFromFile("Resources/9.png"));
      Textures.Add(Texture.LoadFromFile("Resources/..png"));
      Textures.Add(Texture.LoadFromFile("Resources/F.png"));
      Textures.Add(Texture.LoadFromFile("Resources/P.png"));
      Textures.Add(Texture.LoadFromFile("Resources/S.png"));
    }
  }
}
