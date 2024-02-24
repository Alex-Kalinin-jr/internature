#version 330 core

#define NR_POINTLIGHTS 1 // to be passed through uniform
#define NR_FLASHLIGHTS 1 // to be passed
#define NR_DIRECTIONAL_LIGHTS 1


struct DirLight {
    vec3 direction;

    vec3 color;
    vec3 diffuse;
    vec3 specular;
};

struct Material {	
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shiness;
};


in vec3 FragPos;
in vec3 Normal;

uniform vec3 viewPos; //
uniform Material material; //
uniform DirLight dirlights[NR_DIRECTIONAL_LIGHTS]; //

out vec4 outColor;


vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir) {

    vec3 lightDir = normalize(-light.direction);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shiness);

    vec3 ambient = light.color * material.ambient;
    vec3 diffuse = light.diffuse * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;

    return (ambient + diffuse + specular);
}

void main() {	
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 result = vec3(0.0, 0.0, 0.0);

    for (int i = 0; i < NR_DIRECTIONAL_LIGHTS; i++) {
        result += CalcDirLight(dirlights[i], norm, viewDir);
    }

    outColor = vec4(result, 1.0);
}