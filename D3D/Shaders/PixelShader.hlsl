
struct Light
{
    float4 color;
    float3 position;
    float ambientStrength;
    float specularStrength;
    float3 viewPos;
};

cbuffer PsLightConstantBuffer : register(b0)
{
    Light lights[3];
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
    float3 result = float3(0.0f, 0.0f, 0.0f);
    
    for (int i = 0; i < 3; i++)
    {
        float3 ambient = lights[i].ambientStrength * lights[i].color.xyz;
        
        float3 norm = normalize(input.normal);
        float3 lightDir = normalize(lights[i].position - input.fragPos);
        
        float diff = max(dot(norm, lightDir), 0.0f);
        float3 diffuse = diff * lights[i].color.xyz;
        
        float3 viewDir = normalize(lights[i].viewPos - input.fragPos);
        float3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
        float3 specular = lights[i].specularStrength * spec * lights[i].color.xyz;
        
        result += (ambient + diffuse + specular);
    }
    
    result *= volumeColor;
    
    return float4(result, 1.0);
}