using System;

namespace D3D {

  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
      var mouseParams = new CMouseMovingParams(10.0f, 20.0f);
      
      using (var temp = new MyForm(mouseParams)) {
        var pipeScene = Generator.CreatePipeTestingScene(10.0f, -18.0f, 15.0f); // hover func name to reveal values meaning
        DrawSystem.ChangePipeAppearance(0.2f, 10);
        temp.AddPipeScene(pipeScene);
        temp.Run();
      }
    }
  }


}
