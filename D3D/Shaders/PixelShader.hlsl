
cbuffer PsLightConstantBuffer : register(b0) {
    float4 color;
    float3 position;
    float ambientStrength;
    float specularStrength;
    float3 viewPos;
};

struct PSIn
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 fragPos : POSITION1;
};

float4 main(PSIn input) : SV_TARGET
{
    float3 volumeColor = float3(0.3f, 0.3f, 0.3f);
    
    float3 ambient = ambientStrength * color.xyz;
    
    float3 norm = normalize(input.normal);
    float3 lightDir = normalize(position - input.fragPos);
    
    float diff = max(dot(norm, lightDir), 0.0f);
    float3 diffuse = diff * color.xyz;
    
    float3 viewDir = normalize(viewPos - input.fragPos);
    float3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    float3 specular = specularStrength * spec * color.xyz;
    
    float3 result = (ambient + diffuse + specular) * volumeColor;
    
    return float4(result, 1.0);
}