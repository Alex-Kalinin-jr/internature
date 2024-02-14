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
  }
   
  internal class Cube : Volume {

    public Cube(string? path = null) : base() {

      if (path != null) {
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
        Vertices = app.Generator.GenerateCube(10);
      }

      Texture = -1;
      ScaleVr = OpenTK.Mathematics.Vector3.One;
      PosVr = OpenTK.Mathematics.Vector3.Zero;
      RotationVr = OpenTK.Mathematics.Vector3.Zero;
    }
  }
}


