namespace D3D {
  /// <summary>
  /// Component representing transformation data.
  /// </summary>
  public class CTransform : Component {
    /// <summary>
    /// The transformation matrix object.
    /// </summary>
    public VsMvpConstantBuffer TransformObj;

    /// <summary>
    /// Constructs a new instance of CTransform with the provided transformation matrix.
    /// </summary>
    /// <param name="matrix">The transformation matrix.</param>
    public CTransform(VsMvpConstantBuffer matrix) {
      TransformObj = matrix;
      TransformSystem.Register(this);
    }
  }
}
