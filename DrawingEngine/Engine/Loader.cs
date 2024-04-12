using D3D;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class PropertiesReader {
  public static bool LoadProperties(ref CGridMesh mesh, string filePath) {
    using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open))) {
      var size = mesh.Size[0] * mesh.Size[1] * mesh.Size[2];
      string line = reader.ReadLine();

      if (int.TryParse(line, out int numberOfProperties)) {
        for (int i = 0; i < numberOfProperties; ++i) {
          float[] property = new float[size];
          for (int j = 0; j < size; ++j) {
            float.TryParse(reader.ReadLine(), out property[j]);
          }
          string key = "property " + i.ToString();

          float max = property.Max();
          float step = 255.0f / max;
          var propList = new List<Vector3>();
          foreach (var prop in property) {
            propList.Add(new Vector3(step * prop, step * prop, step * prop));
          }
          mesh.AddProperty(key, propList.ToArray());
        }
      } else {
        return false;
      }
    }
    return true;
  }
}

