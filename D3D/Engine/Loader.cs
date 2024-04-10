using SharpDX;
using System.Globalization;
using System.IO;
/*
public class GridBinReader {
  public static CellGrid Read(string path, float scale, float verticalScale = 1.0f, float angle = 0) {
    CellGrid grid = new CellGrid();
    Vector3Int size = new Vector3Int();
    var rotor = Quaternion.AngleAxis(angle, Vector3.up);

    using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open))) {
      size.x = reader.ReadInt32();
      size.y = reader.ReadInt32();
      size.z = reader.ReadInt32();
      grid.size = size;
      for (int k = 0; k < size.z; k++) {
        CellGridLayer layer = new CellGridLayer();
        layer.cells = new GridCell[size.x, size.y];

        for (int i = 0; i < size.x; i++) {
          for (int j = 0; j < size.y; j++) {
            layer.cells[i, j] = new GridCell();
            layer.cells[i, j].active = reader.ReadBoolean();
            for (int corner = 0; corner < 4; corner++) {
              layer.cells[i, j].topCorners[corner].x = reader.ReadSingle() * scale;
              layer.cells[i, j].topCorners[corner].y = -reader.ReadSingle() * scale * verticalScale;
              layer.cells[i, j].topCorners[corner].z = reader.ReadSingle() * scale;

              layer.cells[i, j].topCorners[corner] = rotor * layer.cells[i, j].topCorners[corner];

              layer.cells[i, j].bottomCorners[corner].x = reader.ReadSingle() * scale;
              layer.cells[i, j].bottomCorners[corner].y = -reader.ReadSingle() * scale * verticalScale;
              layer.cells[i, j].bottomCorners[corner].z = reader.ReadSingle() * scale;

              layer.cells[i, j].bottomCorners[corner] = rotor * layer.cells[i, j].bottomCorners[corner];

            }
          }

        }
        grid.layers.Add(layer);
      }
    }


    return grid;
  }

  public static void ReadProperties(string path, CellGrid grid) {
    var lines = File.ReadAllLines(path);
    int lineId = 0;
    int count = int.Parse(lines[lineId++], NumberStyles.Number, CultureInfo.InvariantCulture);
    for (int counter = 0; counter < count; counter++) {
      string name = lines[lineId++];
      var values = new double[grid.size.x, grid.size.y, grid.size.z];
      for (int i = 0; i < grid.size.x; i++) {
        for (int j = 0; j < grid.size.y; j++) {
          for (int k = 0; k < grid.size.z; k++) {
            double value = double.Parse(lines[lineId++], NumberStyles.AllowExponent | NumberStyles.Float,
                CultureInfo.InvariantCulture);
            values[i, j, k] = value;
          }
        }
      }

      grid.properties[name] = values;
    }
  }
}
*/