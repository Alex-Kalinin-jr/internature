using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app {



  internal class FileData {
    public float[]? _vertices;
    public float[]? _texCoords;
    public uint[]? _indices;
    public FileData() {}
  }

  // ////////////////////////////////////////////////////////////////////////////////////////
  static internal class OBJParser {
    static internal FileData ParseFromFile(string filename) {

      List<float> vertices = new List<float>();
      List<float> texCoords = new List<float>();
      List<uint> indices = new List<uint>();

      foreach (string line in File.ReadAllLines(filename)) {
        if (line.StartsWith("#")) continue;

        var tokens = line.Split(' ');

        if (tokens[0] == "v") {
          vertices.Add(float.Parse(tokens[1]));
          vertices.Add(float.Parse(tokens[2]));
          vertices.Add(float.Parse(tokens[3]));
        } else if (tokens[0] == "vt") {
          texCoords.Add(float.Parse(tokens[1]));
          texCoords.Add(float.Parse(tokens[2]));
        } else if (tokens[0] == "f") {
          for (int i = 1; i < tokens.Length; ++i) {
            var facet = tokens[i].Split('/');
            foreach (var f in facet) {
              indices.Add(uint.Parse(f));
            }
          }
        }
      }

      FileData data = new FileData();

      if (vertices.Count != 0) {
        data._vertices = new float[vertices.Count];
        vertices.CopyTo(data._vertices);
      }

      if (texCoords.Count != 0) {
        data._texCoords = new float[texCoords.Count];
        texCoords.CopyTo(data._texCoords);
      }

      if (indices.Count != 0) {
        data._indices = new uint[indices.Count];
        indices.CopyTo(data._indices);
      }

      return data;
    }
  }

}
