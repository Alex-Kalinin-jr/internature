﻿using SharpDX.Windows;
using System.Drawing;

namespace D3D {
  public class CCliceTrackBar : CPositionTrackBar {
    public CCliceTrackBar(RenderForm form, Point position, int yStep) : base(form, position, yStep) { }
  }
}