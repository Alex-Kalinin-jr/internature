struct VSOut
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

cbuffer VS_CONSTANT_BUFFER : register(b0)
{
    float4 cl;
    matrix model;
    matrix view;
    matrix projection;
};

VSOut main(float4 position : POSITION, float4 color : COLOR)
{
    VSOut output;
    matrix mvp = mul(model, view);
    mvp = mul(mvp, projection);
    output.position = mul(mvp, position);
    output.color = cl;

    return output;
}