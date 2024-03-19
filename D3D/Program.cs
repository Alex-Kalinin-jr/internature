using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D {

  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
      using (var temp = new MyForm()) {
        temp.Run();
      }
    }
  }


}
