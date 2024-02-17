using OpenGLInvestigation.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLInvestigation.Entities {
  internal class Storage {

    private List<Volume> _volumes;

    Storage() {
      _volumes = new List<Volume>();
    }

    public void Add(ref Volume fig) {
      _volumes.Add(fig);
    }
  }


}
