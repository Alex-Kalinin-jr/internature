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
      string line = reader.ReadLine();
      CultureInfo culture = CultureInfo.InvariantCulture;

      if (int.TryParse(line, out int numberOfProperties)) {
        for (int i = 0; i < numberOfProperties; ++i) {
          float[] property = new float[size];

          for (int j = 0; j < size; ++j) {
            float val = 0.0f;
            string line2 = reader.ReadLine();

            while (!float.TryParse(line2, NumberStyles.Float, culture, out val)) {
              line2 = reader.ReadLine();
            }
            property[j] = val;
          }

          string key = "property " + i.ToString();
          float max = property.Max();

          for (int k = 0; k < property.Length; ++k) {
            property[k] /= max;
          }

          mesh.AddProperty(key, property);
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
      var pseudoIndices = new List<short>() { 0, 1, 2, 1, 3, 2, 2, 3, 5, 2, 5, 4, 4, 5, 7, 4, 7, 6, 6, 7, 1, 6, 1, 0, 1, 7, 5, 1, 5, 3, 0, 2, 6, 2, 4, 6 };
      var lineIndices = new List<short>();
      var pseudoLineIndices = new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 0, 2, 1, 3, 6, 4, 7, 5 };
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
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //0
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //1
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //2
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //3
            vertices.Add(new VsBuffer(new Vector3(reader.ReadSingle() * scale, reader.ReadSingle() * scale, reader.ReadSingle() * scale), color, i, j, k)); //3

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
        LoadProperties(ref mesh, "grid/grid.binprops.txt");
      }

      return mesh;
    }
  }
}

