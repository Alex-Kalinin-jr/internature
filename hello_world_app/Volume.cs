using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace app {
  internal abstract class Volume {
    public float[]? Vertices { get; init; }
    public float[]? TexCoords {  get; init; }
    public uint[]? Indices { get; init; }
    public int Texture { get; set; }
    public float[]? Colors { get; init; }
    public OpenTK.Mathematics.Vector3 ScaleVr {  get; set; }
    public OpenTK.Mathematics.Vector3 PosVr { get; set; }
    public OpenTK.Mathematics.Vector3 RotationVr { get; set; }

    internal  Matrix4 ComputeModelMatrix() {
      return Matrix4.Identity *
        Matrix4.CreateScale(ScaleVr) * 
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(RotationVr.X)) * 
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(RotationVr.Y)) * 
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(RotationVr.Z)) * 
        Matrix4.CreateTranslation(PosVr);
    }
    
    public Volume() {
      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }

    public Volume(string path) : this() {

      if (path != "") {
        FileData data = app.OBJParser.ParseFromFile(path);

        if (data._vertices != null) {
          Vertices = data._vertices;
        }

        if (data._texCoords != null) {
          TexCoords = data._texCoords;
        }

        if (data._indices != null) {
          Indices = data._indices;
        }

      }

      if (path == null) {
        (Vertices, Indices) = app.Generator.GenerateCube(10);
      }
    }

  }
   
  internal class Cube : Volume {

    public Cube(int verticesInLine) {
      (Vertices, Indices) = app.Generator.GenerateCube(verticesInLine);
      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;

    }
  }
}


