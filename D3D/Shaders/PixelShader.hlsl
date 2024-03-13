
cbuffer PsLightConstantBuffer : register(b0) {
    float4 color;
    float3 position;
    float strength;
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
    float3 volumeColor = float3(0.0f, 1.0f, 0.0f);
    float3 ambient = strength * color.xyz;
    float4 color = float4(ambient * volumeColor, 1.0);
    
    return color;
}