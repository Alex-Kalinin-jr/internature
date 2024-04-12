using D3D;
using SharpDX.Direct3D;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
/*]
class Program {
  static bool LoadProperties(ref CGridMesh mesh, string filePath) {
    using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open))) {
      string line = reader.ReadLine();
      if (int.TryParse(line, out int numberOfProperties)) {
        for (int i = 0; i < numberOfProperties; ++i) {

        }
      } else {
        return false;
      }
    }
  }
}

class Program {

  List<List<int>> properties = new List<List<int>>();

    using (StreamReader reader = new StreamReader(filePath)) {
  string line = reader.ReadLine();
  if (int.TryParse(line, out int numberOfProperties)) {
    // Read sets of values for each property
    for (int prop = 0; prop < numberOfProperties; prop++) {
      List<int> propertyValues = new List<int>();

      string[] values = reader.ReadLine().Split(' ');
      foreach (string value in values) {
        if (int.TryParse(value, out int intValue)) {
          propertyValues.Add(intValue);
        } else {
          Console.WriteLine($"Invalid value: {value}. Skipping.");
        }
      }

      properties.Add(propertyValues);
    }
  } else {
    Console.WriteLine("Invalid format: Could not read the number of properties.");
  }
}

// Display the read properties
Console.WriteLine("Properties read from the file:");
foreach (var propValues in properties) {
  Console.WriteLine(string.Join(", ", propValues));
}
  }
}
 */

