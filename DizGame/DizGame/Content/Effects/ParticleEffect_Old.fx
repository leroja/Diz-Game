float4x4 View;
float4x4 Projection;

texture ParticleTexture;
sampler2D texSampler = sampler_state
{
	texture = <ParticleTexture>;
};
float Time;
float Lifespan;
float2 Size;
float3 wind;
float3 Side;
float3 Up;
float FadeInTime;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Direction : TEXCOORD1;
	float Speed : TEXCOORD2;
	float StartTime : TEXCOORD3;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
    float2 RelativeTime : TEXCOORD1;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float3 position = (float3)input.Position;
	float2 offset = Size * float2((input.UV.x - 0.5f) * 2.0f, -(input.UV.y - 0.5f) * 2.0f);    
    position += offset.x * Side + offset.y * Up;
	float relativeTime = (Time - input.StartTime);    
    output.RelativeTime = relativeTime;
	output.Position = mul(float4(position, 1), mul(View,Projection));    
    output.UV = input.UV;
	return output;

}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

	clip(input.RelativeTime);
	float4 color = tex2D(texSampler, input.UV);
	float d = (float)clamp(1.0f - pow((input.RelativeTime / Lifespan), 10),0, 1);
	d *= (float)clamp((input.RelativeTime / FadeInTime), 0, 1);
	return float4(color * d); 
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}

