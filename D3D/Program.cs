using System;

namespace D3D {

  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
      var mouseParams = new CMouseMovingParams(10.0f, 20.0f);
      
      using (var temp = new MyForm(mouseParams)) {
        var pipeScene = Generator.CreatePipeTestingScene(0.0f, 0.0f, 0.0f);
        DrawSystem.ChangePipeAppearance(0.2f, 10);
        temp.AddPipeScene(pipeScene);

        var grid = Reader.GenerateFromBinary("grid/grid.bin", "grid/grid.binprops.txt", 0.005f);
        temp.AddGridScene(Generator.CreateGridScene(grid), grid.Size);
        temp.Run();
      }
    }
  }


}
