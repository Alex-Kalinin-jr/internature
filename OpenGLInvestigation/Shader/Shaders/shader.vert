#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Color;

const float PI = 3.14159;
const float TWO_PI = PI * 2.0;

uniform float Radius;
uniform float Blend;

vec3 sphere(vec2 domain)
{
    vec3 range;
    range.x = Radius * cos(domain.y) * sin(domain.x);
    range.y = Radius * sin(domain.y) * sin(domain.x);
    range.z = Radius * cos(domain.x);
    return range;
}

void main()
{
	Color = aColor;

    vec2 p0 = aPosition.xy * TWO_PI;
    vec3 normal = sphere(p0);;
    vec3 vertex = Radius * normal;
    vertex = mix(aPosition.xyz, vertex, Blend);

	gl_Position = vec4(vertex, 1.0) * model * view * projection;
    gl_PointSize = 3.0;
}
