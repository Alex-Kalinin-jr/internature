
struct PSIn
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 fragPos : POSITION1;
    float4 color : COLOR;
};
 
float4 main(PSIn input) : SV_TARGET
{
    return input.color;
}