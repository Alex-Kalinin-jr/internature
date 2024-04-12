using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System;

namespace D3D {
  class PipeForm : MainForm {

    public PipeForm() {
      var lbl = AddLabel("pipeMod", "Pipe mod", new System.Drawing.Point(20, 30));
      var btn1 = AddRadioButton("line", "Line", new System.Drawing.Point(20, 50), ChangePipeShowType);
      Controls.Add(btn1);
      var btn2 = AddRadioButton("pipe", "Pipe", new System.Drawing.Point(20, 70), ChangePipeShowType);
      Controls.Add(btn2);

      FormClosing += (sender, e) => {
        e.Cancel = true;
        Hide();
      };
    }

    private void ChangePipeShowType(object sender, EventArgs e) {
      var bttn = sender as RadioButton;
      string[] controls = { "pipeParametersLabel", "pipeSegmentsLabel", "pipeRadiusLabel", "pipeSegments", "pipeRadius" };
      if (bttn.Name == "pipe" && bttn.Checked) {
        DrawSystem.ChangePipeType(FigureType.Pipe);
        SetVisibility(true, controls);
      } else if (bttn.Name == "line" && bttn.Checked) {
        DrawSystem.ChangePipeType(FigureType.Line);
        SetVisibility(false, controls);
      }
    }

  }
}
