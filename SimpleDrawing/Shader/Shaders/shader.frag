#version 330 core

#define NR_POINTLIGHTS 1 // to be passed through uniform
#define NR_FLASHLIGHTS 1 // to be passed
#define NR_DIRECTIONAL_LIGHTS 1

in vec3 Normal;
in vec3 FragPos;

out vec4 outColor;

struct DirLight {
    vec3 direction;

    vec3 color;
    vec3 diffuse;
    vec3 specular;
};


struct PointLight {
    vec3 position;

    vec3 color;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

struct FlashLight {
    vec3 position;
    vec3 direction;

    float cutOff;
    float outerCutOff;

    vec3 color;
    vec3 diffuse;
    vec3 specular;

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

uniform vec3 viewPos;
uniform Material material;
uniform DirLight dirlights[NR_DIRECTIONAL_LIGHTS];
uniform FlashLight flashLights[NR_FLASHLIGHTS];
uniform PointLight pointLights[NR_POINTLIGHTS];


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

vec3 CalcPointLights(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {

    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shiness);
    float distancE = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distancE + distancE * distancE * light.quadratic);

    vec3 ambient = light.color * material.ambient;
    vec3 diffuse = light.diffuse * (diff * material.diffuse);
    vec3 specular = light.specular * (spec * material.specular);

    return attenuation * (ambient + diffuse + specular);
}


vec3 CalcFlashLights(FlashLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {

    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shiness);
    float distancE = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distancE + distancE * distancE * light.quadratic);

    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient  = light.color * material.ambient;
    vec3 diffuse  = light.diffuse * diff * intensity * material.diffuse;
    vec3 specular = light.specular * spec * intensity * material.specular;

    return attenuation * (ambient + diffuse + specular);
}


void main() {	
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 result = vec3(0.0, 0.0, 0.0);

    for (int i = 0; i < NR_DIRECTIONAL_LIGHTS; i++) {
        result += CalcDirLight(dirlights[i], norm, viewDir);
    }

    for (int i = 0; i < NR_POINTLIGHTS; i++) {
        result += CalcPointLights(pointLights[i], norm, FragPos, viewDir);
    }

    for (int i = 0; i < NR_FLASHLIGHTS; i++) {
        result += CalcFlashLights(flashLights[i], norm, FragPos, viewDir);
    }

    outColor = vec4(result, 1.0);
}