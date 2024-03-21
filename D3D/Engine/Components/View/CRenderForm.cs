
using Assimp;
using SharpDX.Windows;
using System.Drawing;

namespace D3D {
  public class CRenderForm : Component {
    public RenderForm RenderFormObj;

    public CRenderForm(Size size) {
      RenderFormObj = new RenderForm();
      RenderFormObj.ClientSize = size;
      RenderFormObj.KeyPreview = true;
      RenderFormObj.AllowUserResizing = false;
      RenderFormObj.SuspendLayout();
      RenderFormObj.Name = "MyForm";
      RenderFormObj.ResumeLayout(false);
    }
  }
}
