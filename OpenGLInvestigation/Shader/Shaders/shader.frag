#version 330 core

in vec3 Color;
out vec4 outColor;
uniform vec3 lightColor;

void main()
{
	outColor = vec4(lightColor * Color, 0.0);
}