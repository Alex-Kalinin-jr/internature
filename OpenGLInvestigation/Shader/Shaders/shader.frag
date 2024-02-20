#version 330 core

in vec3 Color;
in vec3 Normal;
in vec3 FragPos;

out vec4 outColor;

uniform vec3 lightColor;
uniform vec3 lightPos;

void main()
{	float ambientStrength = 0.1;
	vec3 ambient = vec3(ambientStrength * lightColor * Color);

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuze = diff * lightColor;

	vec3 result = (ambient + diffuze) * Color;
	outColor = vec4(result, 1.0);
}