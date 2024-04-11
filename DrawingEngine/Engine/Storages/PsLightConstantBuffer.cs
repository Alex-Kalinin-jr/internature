using SharpDX;
using System.Runtime.InteropServices;

namespace D3D {
  /// <summary>
  /// Struct for storing constant buffer data related to lighting in DirectX applications.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct PsLightConstantBuffer {
    /// <summary>
    /// Color of the light source in RGBA format.
    /// </summary>
    public Vector4 Color;

    /// <summary>
    /// Position of the light source in 3D space.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Strength of ambient light emitted by the light source.
    /// </summary>
    public float AmbientStrength;

    /// <summary>
    /// Strength of specular highlights produced by the light source.
    /// </summary>
    public float SpecularStrength;

    /// <summary>
    /// Position of the camera (view) in 3D space.
    /// </summary>
    public Vector3 ViewPos;

    /// <summary>
    /// Initializes a new instance of the <see cref="PsLightConstantBuffer"/> struct.
    /// </summary>
    /// <param name="color">Color of the light source.</param>
    /// <param name="position">Position of the light source.</param>
    public PsLightConstantBuffer(Vector4 color, Vector3 position) {
      Color = color;
      Position = position;
      AmbientStrength = 0.4f;
      SpecularStrength = 0.5f;
      ViewPos = new Vector3(0, 0, 0);
    }
  }
}
