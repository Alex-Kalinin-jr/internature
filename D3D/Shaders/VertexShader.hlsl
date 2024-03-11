struct VSOut
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

cbuffer VS_CONSTANT_BUFFER : register(b0)
{
    float4 cl;
    matrix vpMatrix;
};

VSOut main(float4 position : POSITION, float4 color : COLOR)
{
    VSOut output;
    output.position = position;
    output.color = cl;

    return output;
}