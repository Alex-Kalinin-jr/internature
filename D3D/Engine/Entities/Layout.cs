using Assimp;
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace D3D {
  public class Layout : Entity {
    public Layout() {
      var size = new CScreenSize();
      AddComponent(size);
      AddComponent(new CMousePos());

      var renderForm = new CRenderForm(new Size(size.Width, size.Height));
      AddComponent(renderForm);

      string text = "W - move forward\nA - move left\nS - move backward\nD - move right\n= - move up\n- - move down\nRMB - movings\nWheel-Pressed - rotation";
      new CLabel(renderForm.IamRenderForm, new Point(25, 25), text);

      new CLabel(renderForm.IamRenderForm, new Point(25, 160), "x-coord");
      new CLabel(renderForm.IamRenderForm, new Point(25, 210), "y-coord");
      new CLabel(renderForm.IamRenderForm, new Point(25, 260), "z-coord");


      var button = new CButton(renderForm.IamRenderForm, new Point(25, 300), "light color");
      AddComponent(button);
      // button.IamButton.Click += ChangeLightColor;
      var trackBar = new CPositionTrackBar(renderForm.IamRenderForm, new System.Drawing.Point(100, 150), 50);
      AddComponent(trackBar);


    }
  }
}
