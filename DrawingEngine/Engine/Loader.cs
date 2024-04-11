using D3D;
using SharpDX.Direct3D;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;

class Program {
  static void Main() {
    string filePath = "grid/grid.bin";
    string filePath2 = "grid/grid.txt";

    using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) {
      int i = reader.ReadInt32();
      int j = reader.ReadInt32();
      int k = reader.ReadInt32();

      float[] floats = new float[6 * i * j * k];
      int index = 0;
      for (int kk = 0; kk < k; ++kk) {
        reader.ReadBoolean();

        for (int ii = 0; ii < i; ++ii) {
          for (int jj = 0; jj < j; ++jj) {
            for (int corner = 0; corner < 4; ++corner) {
              floats[index] = reader.ReadSingle();
              ++index;
            }
          }
        }
      }

      // Write the modified integers and floats back to the file
      using (StreamWriter writer = new StreamWriter(File.Open(filePath2, FileMode.Create))) {
        foreach (var num in floats) {
          writer.Write(num * (1 / 39.0f));
          writer.Write("--");
        }
      }

      Console.WriteLine("Integers and floats have been modified and written back to the file.");
    }
  }
}

