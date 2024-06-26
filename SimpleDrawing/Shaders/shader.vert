#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 2) in vec3 aNormal;


uniform mat4 model; //
uniform mat4 view; //
uniform mat4 projection;
uniform mat4 invertedModel;
uniform float morphingFactor; // 

out vec3 Normal;
out vec3 FragPos;
// out vec3 DistortedPosition;

void main()
{
    FragPos = vec3(vec4(aPos, 1.0) * model);


	float dx = 1.0 - (aPos.y*aPos.y/2.0) - (aPos.z*aPos.z/2.0) + (aPos.y*aPos.y*aPos.z*aPos.z/3.0);
	float dy = 1.0 - (aPos.z*aPos.z/2.0) - (aPos.x*aPos.x/2.0) + (aPos.z*aPos.z*aPos.x*aPos.x/3.0);
	float dz = 1.0 - (aPos.x*aPos.x/2.0) - (aPos.y*aPos.y/2.0) + (aPos.x*aPos.x*aPos.y*aPos.y/3.0);

	float dxX = aPos.x * sqrt(abs(dx)) * morphingFactor + aPos.x * (1.0 - morphingFactor);
	float dyY = aPos.y * sqrt(abs(dy)) * morphingFactor + aPos.y * (1.0 - morphingFactor);
	float dzZ = aPos.z * sqrt(abs(dz)) * morphingFactor + aPos.z * (1.0 - morphingFactor);


	vec3 DistortedPosition = vec3(dxX, dyY, dzZ);

    Normal = (aNormal) * mat3(transpose(invertedModel));

	gl_Position = vec4(DistortedPosition, 1.0) * model * view * projection;
    gl_PointSize = 3.0;
}

