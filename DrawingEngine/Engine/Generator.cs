using SharpDX;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace D3D {
  /// <summary>
  /// Debugging class for generating scenes, meshes, and properties.
  /// </summary>
  public class Generator {

    /// <summary>
    /// Creates another scene for testing pipe figures.
    /// </summary>
    /// <returns>The created scene.</returns>
    public static Scene CreatePipeTestingScene(float worldX, float worldY, float worldZ) {
      var scene = new Scene();
      var mesh = CreateTestingLineMesh();
      mesh.TransformObj.TransformObj.world = TransformSystem.ComputeModelMatrix(new Vector3(0.0f, 0.0f, 0.0f),
                                                                                new Vector3(worldX, worldY, worldZ));
      MeshConverter.CreatePipe(mesh, 0.3f, 15);
      scene.AddComponent(mesh);
      return scene;
    }

    public static Scene CreateGridScene(CGridMesh grid) {
      var scene = new Scene();
      scene.AddComponent(grid);
      return scene;
    }

    /// <summary>
    /// Creates a line mesh for testing.
    /// </summary>
    /// <returns>The created line mesh.</returns>
    public static CMesh CreateTestingLineMesh() {
      List<VsBuffer> vertices = new List<VsBuffer>() {
                new VsBuffer(new Vector3(0.0f, 0.0f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 4.5f, 0.0f)),
                new VsBuffer(new Vector3(0.0f, 10.0f, 0.5f)),
                new VsBuffer(new Vector3(0.0f, 16.5f, -0.5f)),
                new VsBuffer(new Vector3(0.0f, 20.5f, 0.5f))
            };
      var mesh = new CMesh(vertices, new List<short>() { 0, 1, 2, 3, 4 }, FigureType.Line);
      mesh.TransformObj.TransformObj.world = TransformSystem.ComputeModelMatrix(new Vector3(0, 0, 0), new Vector3(5, 0, 5));
      return mesh;
    }

  }
}

