using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;

namespace D3D {
  public class CPipeMesh : CMesh {

    public CMesh RelatedLineMesh;

    public CPipeMesh(CMesh mesh, float pipeRadius, int segments) :  base() {
      RelatedLineMesh = mesh;
      ChangePipe(pipeRadius, segments);
    }

    public void ChangePipe(float pipeRadius, int segments) {
      var buffMesh = MeshConverter.ConvertToPipe(RelatedLineMesh, pipeRadius, segments);
      Vertices = buffMesh.Vertices;
      Indices = buffMesh.Indices;
      TopologyObj = buffMesh.TopologyObj;
      TransformObj = buffMesh.TransformObj;
      FigureTypeObj = buffMesh.FigureTypeObj;
      DrawSystem.Unregister(buffMesh);
    }

  }
}
