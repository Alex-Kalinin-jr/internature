#version 330 core

in vec3 Color;
in vec3 Normal;
in vec3 FragPos;
// in vec3 DistortedPosition;

out vec4 outColor;

uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;

struct Material {	
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shiness;
};

uniform Material material;




void main() {	
// vec3 dpdx = dFdx(DistortedPosition);
// vec3 dpdy = dFdy(DistortedPosition);
// vec3 distortedNormal = normalize(cross(dpdx, dpdy));

// ambient
vec3 ambient = lightColor * material.ambient;

// diffuse 
vec3 norm = normalize(Normal);
vec3 lightDir = normalize(lightPos - FragPos);
float diff = max(dot(norm, lightDir), 0.0);
vec3 diffuse = lightColor * (diff * material.diffuse);

// specular
vec3 viewDir = normalize(viewPos - FragPos);
vec3 reflectDir = reflect(-lightDir, norm);
float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shiness);
vec3 specular = lightColor * (spec * material.specular);

vec3 result = ambient + diffuse + specular;

outColor = vec4(result, 1.0);
}