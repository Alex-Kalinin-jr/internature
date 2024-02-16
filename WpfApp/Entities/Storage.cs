using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app {
  internal class Storage {

    private List<app.Volume> _volumes;

    Storage() {
      _volumes = new List<app.Volume>();
    }

    public void Add(ref Volume fig) {
      _volumes.Add(fig);
    }
  }


}
