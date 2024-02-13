#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
	texCoord = aTexCoord;

	vec3 morphedPosition = mix(aPosition, normalize(aPosition), 0.5f);

	gl_Position = vec4(morphedPosition, 1.0) * model * view * projection;
}