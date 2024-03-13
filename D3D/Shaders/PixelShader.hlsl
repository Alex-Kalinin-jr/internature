
cbuffer PsLightConstantBuffer : register(b0) {
    float4 diffuseColor;
    float3 lightDirection;
    float padding;
};

struct PSIn
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
};

float4 main(PSIn input) : SV_TARGET
{
    float4 volumeColor = float4(0.0f, 1.0f, 0.0f, 1.0f);
    float4 color;
    float3 lightDir;
    float lightIntensity;
    float4 buffcolor;

    lightIntensity = saturate(dot(input.normal, -lightDirection));

    buffcolor = saturate(diffuseColor * lightIntensity);

    color = buffcolor * volumeColor;
    
    return color;
}