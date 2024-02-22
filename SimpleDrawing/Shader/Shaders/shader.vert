#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 2) in vec3 aNormal;


out vec3 Color;
out vec3 Normal;
out vec3 FragPos;

uniform vec3 aColor;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 invertedModel;
uniform float morphingFactor;

void main()
{
    Color = aColor;
    FragPos = vec3(vec4(aPos, 1.0) * model);
    Normal = aNormal * mat3(transpose(invertedModel));


	float dx = 1.0 - (aPos.y*aPos.y/2.0) - (aPos.z*aPos.z/2.0) + (aPos.y*aPos.y*aPos.z*aPos.z/3.0);
	float dy = 1.0 - (aPos.z*aPos.z/2.0) - (aPos.x*aPos.x/2.0) + (aPos.z*aPos.z*aPos.x*aPos.x/3.0);
	float dz = 1.0 - (aPos.x*aPos.x/2.0) - (aPos.y*aPos.y/2.0) + (aPos.x*aPos.x*aPos.y*aPos.y/3.0);

	float dxX = aPos.x * sqrt(abs(dx)) * morphingFactor + aPos.x * (1.0 - morphingFactor);
	float dyY = aPos.y * sqrt(abs(dy)) * morphingFactor + aPos.y * (1.0 - morphingFactor);
	float dzZ = aPos.z * sqrt(abs(dz)) * morphingFactor + aPos.z * (1.0 - morphingFactor);

	vec3 distortedPosition = vec3(dxX, dyY, dzZ);

	gl_Position = vec4(distortedPosition, 1.0) * model * view * projection;
    gl_PointSize = 3.0;
}
