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
//Calculate the final position of each vertex (including moving them to their respective billboard corners). 
//Additionally, the vertex shader will pass through the texture coordinates and will determine the total length 
//of time that the current particle has been active:
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float3 position = input.Position;

	//Move to Billboard corner
	float2 offset = Size * float2((input.UV.x - 0.5f) * 2.0f, -(input.UV.y - 0.5f) * 2.0f);    
    position += offset.x * Side + offset.y * Up;
    //position += offset.x * Size.x * Side + offset.y * Size.y * Up;
    // Determine how long this particle has been alive  
	float relativeTime = (Time - input.StartTime);    
    output.RelativeTime = relativeTime;
	// Move the vertex along its movement direction and the wind  direction
	position += (input.Direction * input.Speed + wind) * relativeTime;
	// Transform the final position by the view and projection matrices  
	output.Position = mul(float4(position, 1), mul(View,Projection));    
    output.UV = input.UV;
	return output;

}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Ignore particles that aren't active 
	clip(input.RelativeTime);
	// Sample texture
	float4 color = tex2D(texSampler, input.UV);
	// Fade out towards end of life
	float d = clamp(1.0f - pow((input.RelativeTime / Lifespan), 10),0, 1);
	// Fade in at beginning of life
	d *= clamp((input.RelativeTime / FadeInTime), 0, 1);
	// Return color * fade amount   
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

