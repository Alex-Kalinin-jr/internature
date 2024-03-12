float4 main(float4 position : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
    float4 colorbuff = float4(color.xyz, 1.0f);
    return color;
}