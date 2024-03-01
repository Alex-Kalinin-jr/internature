#version 330

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoord;

uniform mat4 modelMatrix;

out vec2 texCoord;

void main(void)
{
    texCoord = aTexCoord;

    gl_Position = vec4(aPos, 1.0) * modelMatrix;
}