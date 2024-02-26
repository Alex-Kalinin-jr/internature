using OpenTK.Mathematics;

namespace SimpleDrawing.Entities {
  public class Generator {

    public static (float[], Material, float[]) GenerateCube(int count, Vector3 color) {

      float[] cube = new float[6 * 3 * (count - 1) * (count - 1) * 6];
      float step = 2.0f / (count - 1);
      int ind = 0;

      float[] normals = new float[cube.Length];
      GeneratePlaneYZ(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneYZ(count, -step, 1.0f, ref cube, ref ind, 1.0f);
      GeneratePlaneXZ(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneXZ(count, -step, 1.0f, ref cube, ref ind, 1.0f);
      GeneratePlaneXY(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneXY(count, -step, 1.0f, ref cube, ref ind, 1.0f);

      Material material = new Material();
      material.Ambient = color;

      GenerateNormals(ref cube, ref normals);

      return (cube, material, normals);
    }

    private static void GenerateNormals(ref float[] vertices, ref float[] normals) {

      for (int i = 0; i < vertices.Length; i += 9) {
        Vector3 v1 = new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);
        Vector3 v2 = new Vector3(vertices[i + 3], vertices[i + 4], vertices[i + 5]);
        Vector3 v3 = new Vector3(vertices[i + 6], vertices[i + 7], vertices[i + 8]);
        Vector3 edge1 = v2 - v1;
        Vector3 edge2 = v3 - v1;
        Vector3 triangleNormal = Vector3.Cross(edge1, edge2);

        normals[i] = triangleNormal.X;
        normals[i + 1] = triangleNormal.Y;
        normals[i + 2] = triangleNormal.Z;
        normals[i + 3] = triangleNormal.X;
        normals[i + 4] = triangleNormal.Y;
        normals[i + 5] = triangleNormal.Z;
        normals[i + 6] = triangleNormal.X;
        normals[i + 7] = triangleNormal.Y;
        normals[i + 8] = triangleNormal.Z;
      }

    }


    public static (float[], Material, float[]) GenerateTestingCube() {
      float[] vertices = {
        -0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, 0.5f, -0.5f,
        0.5f, 0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, 0.5f, 0.5f,
        0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f, -0.5f, 0.5f, -0.5f, -0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f, -0.5f, -0.5f,  0.5f, -0.5f, 0.5f, 0.5f,
        0.5f, 0.5f, 0.5f, 0.5f, 0.5f, -0.5f, 0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f, 0.5f, -0.5f,  0.5f, 0.5f,  0.5f, 0.5f,
        -0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, -0.5f, 0.5f,
        0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, -0.5f,
        -0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, 0.5f,
        0.5f,  0.5f, 0.5f, -0.5f,  0.5f, 0.5f, -0.5f,  0.5f, -0.5f
      };

      float[] normals = {
        0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f,
        0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f,
        0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
        -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f
      };

      Material material = new Material();
      material.Ambient = new Vector3(1.0f, 0.5f, 0.0f);

      return (vertices, material, normals);
    }

    private static void GeneratePlaneXY(int count, float step, float xStart,
        ref float[] cube, ref int ind, float zCoord) {

      float stepY = step < 0 ? -step : step;

      float leftX = xStart;
      float rightX = xStart + step;
      float botY = -1.0f;
      float topY = -1.0f + stepY;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = leftX;
          cube[ind + 1] = botY;
          cube[ind + 2] = zCoord;

          cube[ind + 3] = leftX;
          cube[ind + 4] = topY;
          cube[ind + 5] = zCoord;

          cube[ind + 6] = rightX;
          cube[ind + 7] = botY;
          cube[ind + 8] = zCoord;


          cube[ind + 9] = leftX;
          cube[ind + 10] = topY;
          cube[ind + 11] = zCoord;

          cube[ind + 12] = rightX;
          cube[ind + 13] = topY;
          cube[ind + 14] = zCoord;

          cube[ind + 15] = rightX;
          cube[ind + 16] = botY;
          cube[ind + 17] = zCoord;

          ind += 18;
          leftX += step;
          rightX += step;
        }

        rightX = xStart + step;
        leftX = xStart;
        topY += stepY;
        botY += stepY;
      }
    }

    private static void GeneratePlaneYZ(int count, float step, float xStart,
      ref float[] cube, ref int ind, float xCoord) {

      float stepY = step < 0 ? -step : step;

      float leftX = xStart;
      float rightX = xStart + step;
      float botY = -1.0f;
      float topY = -1.0f + stepY;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = xCoord;
          cube[ind + 1] = leftX;
          cube[ind + 2] = botY;

          cube[ind + 3] = xCoord;
          cube[ind + 4] = leftX;
          cube[ind + 5] = topY;

          cube[ind + 6] = xCoord;
          cube[ind + 7] = rightX;
          cube[ind + 8] = botY;


          cube[ind + 9] = xCoord;
          cube[ind + 10] = leftX;
          cube[ind + 11] = topY;

          cube[ind + 12] = xCoord;
          cube[ind + 13] = rightX;
          cube[ind + 14] = topY;

          cube[ind + 15] = xCoord;
          cube[ind + 16] = rightX;
          cube[ind + 17] = botY;

          ind += 18;
          leftX += step;
          rightX += step;
        }
        leftX = xStart;
        rightX = xStart + step;
        topY += stepY;
        botY += stepY;
      }
    }

    private static void GeneratePlaneXZ(int count, float step, float xStart,
      ref float[] cube, ref int ind, float yCoord) {

      float stepY = step < 0 ? -step : step;

      float leftX = xStart;
      float rightX = xStart + step;

      float botY = -1.0f;
      float topY = -1.0f + stepY;


      for (int i = 0; i < count - 1; ++i) {
        for (int j = 0; j < count - 1; ++j) {
          cube[ind] = leftX;
          cube[ind + 1] = yCoord;
          cube[ind + 2] = botY;

          cube[ind + 3] = rightX;
          cube[ind + 4] = yCoord;
          cube[ind + 5] = botY;

          cube[ind + 6] = leftX;
          cube[ind + 7] = yCoord;
          cube[ind + 8] = topY;


          cube[ind + 9] = leftX;
          cube[ind + 10] = yCoord;
          cube[ind + 11] = topY;

          cube[ind + 12] = rightX;
          cube[ind + 13] = yCoord;
          cube[ind + 14] = botY;

          cube[ind + 15] = rightX;
          cube[ind + 16] = yCoord;
          cube[ind + 17] = topY;


          ind += 18;
          rightX += step;
          leftX += step;
        }

        leftX = xStart;
        rightX = xStart + step;
        topY += stepY;
        botY += stepY;
      }
    }


    public static List<Cube> GenerateVolumes() {
      var volumes = new List<Cube>();

      for (float i = -4.0f; i <= 4.0f; i += 2.0f) {
        for (float j = -4.0f; j <= 4.0f; j += 2.0f) {
          Cube buff = new Cube(4, new Vector3(0.2125f, 0.1275f, 0.054f));
          buff.PosVr += new Vector3(i, 1.0f, j);
          volumes.Add(buff);
        }
      }
      return volumes;
    }

    public static List<Light> GenerateLights() {
      var lights = new List<Light>();

      var dirLight = new DirectionalLight();
      dirLight.Direction = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
      dirLight.Form.PosVr = new Vector3(0.0f, -3.0f, 1.0f);
      dirLight.Form.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f);

      var pointLight = new PointLight();
      pointLight.Form.PosVr = new Vector3(0.0f, -1.5f, -1.0f);
      pointLight.Form.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f);

      var flashLight = new FlashLight();
      flashLight.Form.PosVr = new Vector3(0.0f, 0.5f, 6.0f);
      flashLight.Direction = new System.Numerics.Vector3(0.0f, 0.0f, -1.0f);
      flashLight.Form.ScaleVr = new Vector3(0.1f, 0.1f, 0.1f);

      lights.Add(dirLight);
      lights.Add(pointLight);
      lights.Add(flashLight);

      return lights;
    }

  }
}
