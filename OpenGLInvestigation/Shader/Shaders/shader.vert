#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

uniform float morphingFactor;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Color;

void main()
{
	Color = aColor;

	float dx = 1.0 - (aPosition.y*aPosition.y/2.0) - (aPosition.z*aPosition.z/2.0) + (aPosition.y*aPosition.y*aPosition.z*aPosition.z/3.0);
	float dy = 1.0 - (aPosition.z*aPosition.z/2.0) - (aPosition.x*aPosition.x/2.0) + (aPosition.z*aPosition.z*aPosition.x*aPosition.x/3.0);
	float dz = 1.0 - (aPosition.x*aPosition.x/2.0) - (aPosition.y*aPosition.y/2.0) + (aPosition.x*aPosition.x*aPosition.y*aPosition.y/3.0);

	float dxX = aPosition.x * sqrt(abs(dx)) * morphingFactor + aPosition.x * (1.0 - morphingFactor);
	float dyY = aPosition.y * sqrt(abs(dy)) * morphingFactor + aPosition.y * (1.0 - morphingFactor);
	float dzZ = aPosition.z * sqrt(abs(dz)) * morphingFactor + aPosition.z * (1.0 - morphingFactor);

	vec3 distortedPosition = vec3(dxX, dyY, dzZ);

	gl_Position = vec4(distortedPosition, 1.0) * model * view * projection;
    gl_PointSize = 3.0;
}
