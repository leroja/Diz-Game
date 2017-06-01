#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix View;
matrix Projection;

//Lighting
// This sample uses a simple Lambert lighting model.
//Start hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233
float3 LightDirection = normalize(float3(1, 1, 1));
float3 DiffuseLight = 1.25;
float3 AmbientLight = 0.35;
//Slut hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233


Texture Texture;

sampler TextureSampler = sampler_state
{
	texture = <Texture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;

	AddressU = mirror;
	AddressV = mirror;
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float2 TexCoord: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float2 TexCoord: TEXCOORD0;
	float4 Color: COLOR0;
};


//VertexShader for instanced objects (will need a objMatrice per instance).
VertexShaderOutput InstanceVS(in float4 position : SV_POSITION,
							  in VertexShaderInput input,
							  in float4x4 objWorld : TEXCOORD1)
{

	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(position, mul(transpose(objWorld), mul(World, mul(View, Projection))));
	//output.Position = output.Position + perInstancePos;

	output.TexCoord = input.TexCoord;

	//// Compute lighting, using a simple Lambert model.
	////Start hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233
	float3 worldNormal = mul(input.Normal, objWorld);
	float diffuseAmount = max(-dot(input.Normal, LightDirection), 0);
	float3 lightingResult = saturate(diffuseAmount * DiffuseLight + AmbientLight);
	output.Color = float4(lightingResult, 1);
	////Slut hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233

	return output;
}


//VertexShader for ordinary graphics.
VertexShaderOutput MainVS(in float4 position : SV_POSITION,
	in VertexShaderInput input)
{

	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(position, mul(World, mul(View, Projection)));
	output.TexCoord = input.TexCoord;

	//// Compute lighting, using a simple Lambert model.
	////Start hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233
	////float3 worldNormal = mul(input.Normal, objWorld);
	//float diffuseAmount = max(-dot(input.Normal, LightDirection), 0);
	//float3 lightingResult = saturate(diffuseAmount * DiffuseLight + AmbientLight);
	//output.Color = float4(lightingResult, 1);
	////Slut hämtat från http://community.monogame.net/t/help-cant-use-setinstancebuffer/8233

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{

	float4 output = tex2D(TextureSampler, input.TexCoord) * input.Color;

	return output;
}

technique BasicVertexPositionNormalDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};


technique InstancedVertexPositionNormalDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL InstanceVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};