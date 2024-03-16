using System.Collections.Generic;
using System;

namespace D3D {
  public class Figure : Component {
    public Mesh Mesh;


    public Figure(string path, List<CLight> lights) {
      Mesh = new Mesh(path);
      
      DrawingSystem.Register(this);
    }

    public override void Update() {
      var renderer = Renderer.GetRenderer();
      if (renderer != null) {

      }
    }
  }
}
