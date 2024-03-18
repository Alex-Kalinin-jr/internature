
using Assimp;
using SharpDX.Windows;
using System.Drawing;

namespace D3D {
  public class CRenderForm : Component {
    public RenderForm IamRenderForm;

    public CRenderForm(Size size) {
      IamRenderForm = new RenderForm();
      IamRenderForm.ClientSize = size;
      IamRenderForm.KeyPreview = true;
      IamRenderForm.AllowUserResizing = false;
      IamRenderForm.SuspendLayout();
      IamRenderForm.Name = "MyForm";
      IamRenderForm.ResumeLayout(false);
    }
  }
}
