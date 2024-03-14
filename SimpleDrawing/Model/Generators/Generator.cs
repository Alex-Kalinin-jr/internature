using OpenTK.Mathematics;

namespace SimpleDrawing.Model {
  public class Generator {

    public static Form GenerateCubeForm(int count) {
      Form form = new Form();

      int numOfEdges = 6;
      int rangOf3dSystem = 3;
      int trianglePointCount = 3;
      int pseudoTesselationCount = 2;
      float[] cube = new float[numOfEdges * rangOf3dSystem * (count - 1) * (count - 1) * trianglePointCount * pseudoTesselationCount];
      float[] normals = new float[cube.Length];
      float step = 2.0f / (count - 1);
      int ind = 0;

      GeneratePlaneYz(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneYz(count, -step, 1.0f, ref cube, ref ind, 1.0f);
      GeneratePlaneXz(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneXz(count, -step, 1.0f, ref cube, ref ind, 1.0f);
      GeneratePlaneXy(count, step, -1.0f, ref cube, ref ind, -1.0f);
      GeneratePlaneXy(count, -step, 1.0f, ref cube, ref ind, 1.0f);
      GenerateNormals(ref cube, ref normals);

      form.Vertices = cube;
      form.Normals = normals;

      return form;
    }

    public static Form GenerateCubeForm() {
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

      Form form = new Form();
      form.Vertices = vertices;
      form.Normals = normals;

      return form;
    }

    public static List<Cube> GenerateVolumes(int countOfSide, float step) {
      var volumes = new List<Cube>();
      float range = countOfSide * step;
      string[] materials = {"emerald", "obsidian", "chrome", "blackRubber", "bronze"};
      int count = 0;
      for (float i = -range / 2; i < range / 2; i += step) {
        for (float j = -range / 2; j < range / 2; j += step) {
          Cube buff = new Cube(3);
          int a = count % materials.Length;
          buff.ItsMaterial = MaterialColor.CreateMaterial(materials[a]);
          buff.ItsPosition.PosVr += new Vector3(i, 1.0f, j);
          volumes.Add(buff);
          ++count;
        }
      }
      return volumes;
    }

    public static (List<Light>, List<Light>, List<Light>) GenerateLights(int sideCount, float step) {
      var dirLights = new List<Light>();
      var pointLights = new List<Light>();
      var flashLights = new List<Light>();


      var dirLight = new DirectionalLight();
      ((DirectionalLightColor)dirLight.ItsColor).Direction = new Vector3(0.0f, 1.0f, 0.0f);
      dirLight.ItsVolume.ItsPosition.PosVr = new Vector3(0.0f, -3.0f, 1.0f);
      dirLights.Add(dirLight);

      var pointLight = new PointLight();
      pointLight.ItsVolume.ItsPosition.PosVr = new Vector3(0.0f, -1.5f, -1.0f);
      pointLights.Add(pointLight);

      flashLights.AddRange(GenerateFlashLights(sideCount, step));

      return (dirLights, pointLights, flashLights);
    }
    public static List<Light> GenerateFlashLights(int sideCount, float step) {
      var lights = new List<Light>();
      float range = sideCount * step;

      for (float i = -range / 2; i < range / 2; i += step) {
        for (float j = -range / 2; j < range / 2; j += step) {
          var flashLight = new FlashLight();
          flashLight.ItsVolume.ItsPosition.PosVr = new Vector3(i, -2.5f, j);
          lights.Add(flashLight);
        }
      }

      return lights;
    } 

    private static void GenerateNormals(ref float[] vertices, ref float[] normals) {

      for (int i = 0; i < vertices.Length; i += 9) {
        OpenTK.Mathematics.Vector3 v1 = new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);

        OpenTK.Mathematics.Vector3 v2 = new Vector3(vertices[i + 3], vertices[i + 4], vertices[i + 5]);
        OpenTK.Mathematics.Vector3 v3 = new Vector3(vertices[i + 6], vertices[i + 7], vertices[i + 8]);
        OpenTK.Mathematics.Vector3 edge1 = v2 - v1;
        OpenTK.Mathematics.Vector3 edge2 = v3 - v1;
        OpenTK.Mathematics.Vector3 triangleNormal = OpenTK.Mathematics.Vector3.Cross(edge1, edge2);

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


    private static void GeneratePlaneXy(int count, float step, float xStart,
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


    private static void GeneratePlaneYz(int count, float step, float xStart,
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


    private static void GeneratePlaneXz(int count, float step, float xStart,
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
  }
}
