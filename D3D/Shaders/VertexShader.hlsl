struct VSOut
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 fragPos : POSITION1;
    float3 color : COLOR;
};

struct VSIn
{
    float4 position : POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 color : COLOR;
};

cbuffer VsMvpConstantBuffer : register(b0)
{
    Matrix world;
    Matrix view;
    Matrix projection;
};


VSOut main(VSIn input)
{
    input.position.w = 1.0f;
    
    VSOut output;
    
    output.position = mul(input.position, world);
    output.fragPos = output.position.xyz;
    output.position = mul(output.position, view);
    output.position = mul(output.position, projection);
    
    
    output.tex = input.tex;
    output.normal = normalize(mul(input.normal, (float3x3) world));
    output.color = input.color;

    return output;
}