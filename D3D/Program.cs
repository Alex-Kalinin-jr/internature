using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D {

  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
      var verts = Generator.GenerateTestPipe();
      var vertsVector = Generator.Convert(verts);
      (var vertices, var indices) = Generator.GeneratePipe(vertsVector);
      using (var temp = new MyForm()) {
        temp.Run();
      }
    }
  }


}
