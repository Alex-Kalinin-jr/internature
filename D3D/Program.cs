using System;

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
