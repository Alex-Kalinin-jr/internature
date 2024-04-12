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

      var lbl1 = AddLabel("pipeParametersLabel", "Pipe parameters", new System.Drawing.Point(80, 30));
      Controls.Add(lbl1);
      var lbl2 = AddLabel("pipeSegmentsLabel", "Segments", new System.Drawing.Point(80, 60));
      Controls.Add(lbl2);
      var lbl3 = AddLabel("pipeRadiusLabel", "Radius", new System.Drawing.Point(80, 100));
      Controls.Add(lbl3);

      var bar1 = AddTrackbar("pipeSegments", new System.Drawing.Point(130, 60), ChangePipeParameters, 10, 40, 1);
      Controls.Add(bar1);
      var bar2 = AddTrackbar("pipeRadius", new System.Drawing.Point(130, 100), ChangePipeParameters, 1, 20, 1);
      Controls.Add(bar2);

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

    /// <summary>
    /// Changes the parameters of a pipe based on the trackbars for segments and radius.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ChangePipeParameters(Object sender, EventArgs e) {
      TrackBar segments = null;
      TrackBar radius = null;

      var controls = Controls.Find("pipeSegments", true);
      if (controls.Length != 0) {
        foreach (var control in controls) {
          if (control is TrackBar) {
            segments = (TrackBar)control;
            break;
          }
        }
      }

      var radiusControls = Controls.Find("pipeRadius", true);
      if (radiusControls.Length != 0) {
        foreach (var control in radiusControls) {
          if (control is TrackBar) {
            radius = (TrackBar)control;
            break;
          }
        }
      }

      if (!(segments is null) && !(radius is null)) {
        DrawSystem.ChangePipeAppearance(radius.Value / 10.0f, segments.Value);
      }
    }



  }
}
