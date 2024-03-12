using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3D {
  public class Generator {

    public static (Vertex[], short[]) GenerateCube() {
      var _vertices = new Vertex[24];

      _vertices[0] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[1] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[2] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[3] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[4] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[5] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[6] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));
      _vertices[7] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(1.0f, 1.0f, 1.0f, 1.0f));

      _vertices[8] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[9] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[10] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[11] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[12] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[13] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[14] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[15] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[16] = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[17] = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[18] = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[19] = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));

      _vertices[20] = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[21] = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[22] = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));
      _vertices[23] = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 1.0f));


      var _indices = new short[] {
                // front face
                0, 1, 2, // first triangle
                0, 3, 1, // second triangle

                // left face
                4, 5, 6, // first triangle
                4, 7, 5, // second triangle

                // right face
                8, 9, 10, // first triangle
                8, 11, 9, // second triangle

                // back face
                12, 13, 14, // first triangle
                12, 15, 13, // second triangle

                // top face
                16, 17, 18, // first triangle
                16, 19, 17, // second triangle

                // bottom face
                20, 21, 22, // first triangle
                20, 23, 21, // second triangle
      };

      return (_vertices, _indices);
    }
  }
}
