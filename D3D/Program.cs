using System;

namespace D3D {

  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
      var mouseParams = new CMouseMovingParams(10.0f, 20.0f);
      
      using (var temp = new MyForm(mouseParams)) {
        var pipeScene = Generator.CreatePipeTestingScene();
        DrawSystem.ChangePipeAppearance(0.2f, 10);
        temp.AddPipeScene(pipeScene);

        int[] gridSize = { 20, 10, 20 };
        var gridScene = Generator.CreateGridTestingScene(gridSize);
        temp.AddGridScene(gridScene, gridSize);

        temp.Run();
      }
    }
  }


}
