using D3D;
using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Reader {

  public static bool LoadProperties(ref CGridMesh mesh, string filePath) {
    using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open))) {
      var size = mesh.Size[0] * mesh.Size[1] * mesh.Size[2];
      CultureInfo culture = CultureInfo.InvariantCulture;

      if (int.TryParse(reader.ReadLine(), out int numberOfProperties)) {
        for (int i = 0; i < numberOfProperties; ++i) {
          float[] property = new float[size];

          string name = reader.ReadLine();
          for (int j = 0; j < size; ++j) {
            float val;
            while (!float.TryParse(reader.ReadLine(), NumberStyles.Float, culture, out val)) { }
            property[j] = val;
          }

          string key = name + i.ToString();
          float max = property.Max();

          for (int k = 0; k < property.Length; ++k) {
            property[k] /= max;
          }


          int sizeX = mesh.Size[0];
          int sizeY = mesh.Size[2];
          int sizeZ = mesh.Size[1];
          float[] newProp = new float[sizeX * sizeY * sizeZ];
          int index = 0;

          for (int a = 0; a < sizeX; ++a) {
            for (int b = 0; b < sizeY; ++b) {
              for (int c = 0; c < sizeZ; ++c) {
                newProp[index] = property[a + b * sizeY + c * sizeZ * sizeY];
                ++index;
              }
            }
          }

          mesh.AddProperty(key, newProp);
        }
      } else {
        return false;
      }
    }
    return true;
  }

  public static CGridMesh GenerateFromBinary(string filePath, string filePropertiesPath, float scale) {
    using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) {
      int xCount = reader.ReadInt32();
      int yCount = reader.ReadInt32();
      int zCount = reader.ReadInt32();

      List<VsBuffer> vertices = new List<VsBuffer>();
      var indices = new List<short>();
      var pseudoIndices = new List<short>() { 0, 1, 2, 1, 3, 2, 2, 3, 5, 2, 5, 4,
                                              4, 5, 7, 4, 7, 6, 6, 7, 1, 6, 1, 0,
                                              1, 7, 5, 1, 5, 3, 0, 2, 6, 2, 4, 6 };
      var lineIndices = new List<short>();
      var pseudoLineIndices = new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 0, 2, 1, 3, 6, 4, 7, 5, 1, 7, 3, 5, 0, 6, 2, 4 };
      var propertyColor = new List<float>();
      var random = new Random();
      int p = 0;

      for (int k = 0; k < zCount; ++k) {
        float property = (float)random.NextDouble(0.0f, 1.0f);
        Vector3 color = new Vector3(property, property, property);
        for (int i = 0; i < xCount; ++i) {
          for (int j = 0; j < yCount; ++j) {
            reader.ReadBoolean();
            var pseudoVertices = new List<VsBuffer>();
            for (int v = 0; v < 8; v++) {
              vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale,
                                                    -reader.ReadSingle() * scale,
                                                    reader.ReadSingle() * scale), color, j, i, k));
            }

            indices.AddRange(pseudoIndices.Select(v => (short)(v + p)));
            lineIndices.AddRange(pseudoLineIndices.Select(v => (short)(p + v)));
            propertyColor.Add(property);
            p += 8;

          }
        }
      }
      var mesh = new CGridMesh(vertices, indices, FigureType.Grid);
      mesh.LineIndices = lineIndices;
      mesh.TopologyObj = PrimitiveTopology.TriangleList;
      mesh.AddProperty("color", propertyColor.ToArray());

      mesh.Size[0] = xCount;
      mesh.Size[1] = yCount;
      mesh.Size[2] = zCount;

      if (filePropertiesPath != "") {
        LoadProperties(ref mesh, filePropertiesPath);
      }

      return mesh;
    }
  }
}


