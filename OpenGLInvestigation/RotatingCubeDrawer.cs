﻿using System.Diagnostics;
using OpenGLInvestigation.Entities;
using OpenGLInvestigation.Figures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ImGuiNET.OpenTK.Sample;

public delegate void Show(int i);

public sealed class RotatingCubeDrawer {
  private int _width;
  private int _height;

  private Matrix4 _view;
  private Matrix4 _projection;
  private readonly float _FOV = 45.0f;

  //  ///////////////////////////////////////////////////////////////////////////
  private List<Volume> _volumes = new List<Volume>();
  private List<int> _vertexBufferObjects = new List<int>();
  private List<int> _colorBufferObjects = new List<int>();
  private List<int> _vertexArrayObjects = new List<int>();
  private OpenGLInvestigation.Shader _shader;
  private List<int> _elementBufferObjects = new List<int>();
  private Show _showType;
  //  ///////////////////////////////////////////////////////////////////////////

  //  ///////////////////////////////////////////////////////////////////////////

  public void MoveRight() {
    foreach (var item in _volumes) {
      item.PosVr += new Vector3(1.0f, 0.0f, 0.0f);
    }
  }

  public void MoveLeft() {
    foreach (var item in _volumes) {
      item.PosVr += new Vector3(-1.0f, 0.0f, 0.0f);
    }
  }

  public void MoveTop() {
    foreach (var item in _volumes) {
      item.PosVr += new Vector3(0.0f, -1.0f, 0.0f);
    }
  }

  public void MoveBottom() {
    foreach (var item in _volumes) {
      item.PosVr += new Vector3(0.0f, 1.0f, 0.0f);
    }
  }


  public void OnLoad() {

    //  ///////////////////////////////////////////////////////////////////////////
    ChangeDrawingType(0);
    GL.Enable(EnableCap.ProgramPointSize);

    Cube buff = new Cube(10, new Vector3(1.0f, 0.0f, 0.0f));
    Cube buff2 = new Cube (10, new Vector3(1.0f, 1.0f, 0.0f));
    buff2.PosVr += new Vector3(3.0f, 0.0f, 0.0f);
    _volumes.Add(buff);
    _volumes.Add(buff2);

    _shader = new OpenGLInvestigation.Shader("Shader/Shaders/shader.vert", "Shader/Shaders/shader.frag");
    _shader.Use();

    for (int i = 0; i < _volumes.Count; ++i) {
      _vertexArrayObjects.Add(GL.GenVertexArray());
      GL.BindVertexArray(_vertexArrayObjects[i]);

      if (_volumes[i].Vertices != null) {
        BindPosBuffer(i);
      }

      if (_volumes[i].Colors != null) {
        BindColorBuffer(i);
      }

      if (_volumes[i].Indices != null) {
        BindIndicesBuffer(i);
      }
    }
//  //////////////////////////////////////////////////////////////////////////////

    GL.Enable(EnableCap.DepthTest);
    GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
  }

  public void OnClosed() { }

  public void OnResize(int width, int height) {
    _width = width;
    _height = height;

    GL.Viewport(0, 0, _width, _height);
  }

  public void OnRenderFrame() {
//  //////////////////////////////////////////////////////////////////////////////
    AdjustShader();

    for (int i = 0; i < _volumes.Count; ++i) {

      _shader.Use();
      Matrix4 model = _volumes[i].ComputeModelMatrix();
      _shader.SetMatrix4("model", model);

      GL.BindVertexArray(_vertexArrayObjects[i]);

// delegate
      _showType(i);
    }
//  //////////////////////////////////////////////////////////////////////////////
  }

  private void BindPosBuffer(int indexOfDescriptros) {
    _vertexBufferObjects.Add(GL.GenBuffer());
    int vertexLocation = GL.GetAttribLocation(_shader.Handle, "aPosition");
    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjects[0]);
    GL.BufferData(BufferTarget.ArrayBuffer,
        _volumes[indexOfDescriptros].Vertices.Length * sizeof(float),
        _volumes[indexOfDescriptros].Vertices, BufferUsageHint.DynamicDraw);
    GL.EnableVertexAttribArray(vertexLocation);
    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }
  private void BindColorBuffer(int indexOfDescriptros) {
    _colorBufferObjects.Add(GL.GenBuffer());
    int colorLocation = GL.GetAttribLocation(_shader.Handle, "aColor");
    GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObjects[indexOfDescriptros]);
    GL.BufferData(BufferTarget.ArrayBuffer,
      _volumes[indexOfDescriptros].Colors.Length * sizeof(float),
      _volumes[indexOfDescriptros].Colors, BufferUsageHint.StaticDraw);
    GL.EnableVertexAttribArray(colorLocation);
    GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float,
        false, 3 * sizeof(float), 0);
  }
  private void BindIndicesBuffer(int indexOfDescriptros) {
    _elementBufferObjects.Add(GL.GenBuffer());
    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjects[indexOfDescriptros]);
    GL.BufferData(BufferTarget.ElementArrayBuffer, _volumes[indexOfDescriptros].Indices.Length * sizeof(uint),
        _volumes[indexOfDescriptros].Indices, BufferUsageHint.StaticDraw);
  }
  private void AdjustShader() {
    _view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 10.0f), new Vector3(1.5f, 2.0f, 0.0f), Vector3.UnitY);
    _projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * (_FOV / 180f), _width / (float)_height, 0.2f, 256.0f);

    _shader.SetMatrix4("view", _view);
    _shader.SetMatrix4("projection", _projection);
    _shader.SetFloat("Radius", 1.0f);
    _shader.SetFloat("Blend", 0.0f);
  }

  private void ShowSolid(int i) {
    if (_volumes[i].Indices != null) {
      GL.DrawElements(PrimitiveType.Triangles, _volumes[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
    } else {
      GL.DrawArrays(PrimitiveType.Triangles, 0, _volumes[i].Vertices.Length / 3);
    }
  }

  private void ShowFramed(int i) {
    GL.DrawArrays(PrimitiveType.LineStrip, 0, _volumes[i].Indices.Length / 3);
  }

  private void ShowPoints(int i) {
    GL.DrawArrays(PrimitiveType.Points, 0, _volumes[i].Indices.Length / 3);
  }

  public void ChangeDrawingType(int i) {
    if (i == 0) {
      _showType = this.ShowSolid;
    }

    if (i == 1) {
      _showType = this.ShowFramed;
    }

    if (i == 2) {
      _showType = this.ShowPoints;
    }
  }

}