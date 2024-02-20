#version 330 core

in vec3 Color;
out vec4 outColor;

uniform vec3 lightColor;
uniform vec3 lightPos;

void main()
{	float ambientStrength = 0.1;
	// +lightPos is wrong
	outColor = vec4(ambientStrength * lightColor * Color + lightPos, 1.0);
}