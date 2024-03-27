struct VSOut
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 fragPos : POSITION1;
    float4 color : COLOR;
};

struct VSIn
{
    float4 position : POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 color : COLOR;
    int3 coords : GRIDCOORDS;
};

cbuffer VsMvpConstantBuffer : register(b0)
{
    Matrix world;
    Matrix view;
    Matrix projection;
};


cbuffer VsSliceConstantBuffer : register(b1)
{
    int4 slice;
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
    

    if (input.coords[0] == -1 || input.coords[1] == -1 || input.coords[2] == -1 ||
        (slice[0] == -1 && slice[1] == -1 && slice[2] == -1) ||
        (slice[0] == input.coords[0] && slice[0] != -1) ||
        (slice[1] == input.coords[1] && slice[1] != -1) ||
        (slice[2] == input.coords[2] && slice[2] != -1))
    {
        if (slice[3] == 0)
        {
            output.color = float4(0.8, 0.8, 0.8, 0.3);
        }
        else
        {
            output.color = float4(input.color, 1.0);
        }
    }
    else
    {
        output.color = float4(0.0, 0.0, 0.0, 0.0);

    }

        

    
    return output;
}