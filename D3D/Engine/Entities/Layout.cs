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

      var button = new CButton(renderForm.IamRenderForm, new System.Drawing.Point(25, 300), "light color");
      button.IamButton.Click += ChangeLightColor;

      new CLabel(renderForm.IamRenderForm, new System.Drawing.Point(25, 160), "x-coord");
      var xTrackBar = new CPositionTrackBar(renderForm.IamRenderForm, new System.Drawing.Point(100, 150));
      xTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

      new CLabel(renderForm.IamRenderForm, new System.Drawing.Point(25, 210), "y-coord");
      var yTrackBar = new CPositionTrackBar(renderForm.IamRenderForm, new System.Drawing.Point(100, 200));
      yTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

      new CLabel(_renderForm.IamRenderForm, new System.Drawing.Point(25, 260), "z-coord");
      var zTrackBar = new CPositionTrackBar(renderForm.IamRenderForm, new System.Drawing.Point(100, 250));
      zTrackBar.IamTrackBar.Scroll += ChangeLightPosition;

    }
  }
}
