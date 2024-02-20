#version 330 core

in vec3 Color;
out vec4 outColor;
uniform vec3 lightColor;

void main()
{	float ambientStrength = 0.1;

	outColor = vec4(ambientStrength * lightColor * Color, 1.0);
}