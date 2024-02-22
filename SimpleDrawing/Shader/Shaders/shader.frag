#version 330 core

in vec3 Color;
in vec3 Normal;
in vec3 FragPos;
// in vec3 DistortedPosition;

out vec4 outColor;


struct Light {
    vec3 position;
    vec3 color;

    float constant;
    float linear;
    float quadratic;
};



struct Material {	
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shiness;
};

uniform Material material;
uniform Light light;
uniform vec3 viewPos;




void main() {	
// vec3 dpdx = dFdx(DistortedPosition);
// vec3 dpdy = dFdy(DistortedPosition);
// vec3 distortedNormal = normalize(cross(dpdx, dpdy));

// ambient
vec3 ambient = light.color * material.ambient;

// diffuse 
vec3 norm = normalize(Normal);

vec3 lightDir = normalize(light.position - FragPos);
float distancE = length(light.position - FragPos);
float attenuation = 1.0 / (light.constant + light.linear * distancE + distancE * distancE * light.quadratic);
// vec3 lightDir = normalize(-lightPos); // directional light. there should be direction vector!
float diff = max(dot(norm, lightDir), 0.0);
vec3 diffuse = light.color * (diff * material.diffuse);

// specular
vec3 viewDir = normalize(viewPos - FragPos);
vec3 reflectDir = reflect(-lightDir, norm);
float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shiness);
vec3 specular = light.color * (spec * material.specular);

vec3 result = attenuation * (ambient + diffuse + specular);

outColor = vec4(result, 1.0);
}