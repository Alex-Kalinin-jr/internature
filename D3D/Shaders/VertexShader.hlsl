struct VSOut
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

cbuffer VS_CONSTANT_BUFFER : register(b0)
{
    Matrix world;
    Matrix view;
    Matrix projection;
};

VSOut main(float4 position : POSITION, float4 color : COLOR)
{
    VSOut output;
    position.w = 1.0;
    
    output.position = mul(position, world);
    output.position = mul(output.position, view);
    output.position = mul(output.position, projection);
    
    
    output.color = color;

    return output;
}