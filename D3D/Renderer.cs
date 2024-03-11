using D3D;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3D {
  public class Renderer {


  }
}



/*
// Update the model transform buffer for the hologram.
context->UpdateSubresource(
    m_modelConstantBuffer.Get(),
    0,
    nullptr,
    &m_modelConstantBufferData,
    0,
    0
);
Matrix world = Matrix.Identity;
Vector3 eyePos = new Vector3(0.0f, 0.0f, -2.0f);
Vector3 lookAtPos = new Vector3(0.0f, 0.0f, 0.0f);
Vector3 upVector = new Vector3(0.0f, 1.0f, 0.0f);
Matrix view = Matrix.LookAtLH(eyePos, lookAtPos, upVector);

float fovDegrees = 90.0f;
float fovRadians = (fovDegrees / 360.0f) * MathUtil.Pi * 2;
float aspectRatio = (float)Width / Height;
float nearZ = 0.1f;
float farZ = 1000.0f;
Matrix projection = Matrix.PerspectiveFovLH(fovRadians, aspectRatio, nearZ, farZ);

VS_CONSTANT_BUFFER tmp = new VS_CONSTANT_BUFFER();
tmp.cl = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
tmp.vpMatrix = world * view * projection;
tmp.vpMatrix.Transpose();
*/