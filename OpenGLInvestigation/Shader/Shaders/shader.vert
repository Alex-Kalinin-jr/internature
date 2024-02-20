#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec3 aNormal;

out vec3 Color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    
    
    Color = aColor;
    // - aNormal + aNormal wrong.
    gl_Position = vec4(aPos - aNormal, 1.0) * model * view * projection;
}