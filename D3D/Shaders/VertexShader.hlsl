struct VSOut
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
};

struct VSIn
{
    float4 position : POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
};

cbuffer VS_CONSTANT_BUFFER : register(b0)
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
    output.position = mul(output.position, view);
    output.position = mul(output.position, projection);
    
    
    output.tex = input.tex;
    output.normal = normalize(mul(input.normal, (float3x3) world));

    return output;
}