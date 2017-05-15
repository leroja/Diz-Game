float4x4 WorldViewProjection;
float4x4 LightsWorldViewProjection;
float4x4 World;
float3 LightPos;
float LightPower;
float Ambient;

Texture theTexture;
Texture ShadowMap;

//This shader is note done, Shadow Mapping doesn't work yet...
//Working on it! :D
sampler TextureSamp = sampler_state
{
	texture = <theTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};
sampler ShadowMapSampler = sampler_state
{
	texture = <ShadowMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

struct VertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};



float DotProduct(float3 lightPos, float3 pos3D, float3 normal) {
	float3 lightDirr = normalize(pos3D - lightPos);
	return dot(-lightDirr, normal);
}
VertexToPixel SimplestVertexShader(float4 inPos : POSITION0, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = mul(inPos, WorldViewProjection);
	Output.TexCoords = inTexCoords;
	Output.Normal = normalize(mul(inNormal, (float3x3)World));
	Output.Position3D = mul(inPos, World);

	return Output;
}
PixelToFrame FirstPixelShader(VertexToPixel PSIn)
{
	PixelToFrame output = (PixelToFrame)0;
	float diffuseLightingFactor = DotProduct(LightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= LightPower;

	PSIn.TexCoords.y--;
	float4 baseColor = tex2D(TextureSamp, PSIn.TexCoords);
	output.Color = baseColor * (diffuseLightingFactor + Ambient);
	return output;
}
technique Simple
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 SimplestVertexShader();
		PixelShader = compile ps_4_0 FirstPixelShader();
	}
}


struct ShadowMapVertexToPixel
{
	float4 Position : POSITION;
	float4 Position2D : TEXCOORD0;
};

ShadowMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
	ShadowMapVertexToPixel output = (ShadowMapVertexToPixel)0;
	output.Position = mul(inPos, LightsWorldViewProjection);
	output.Position2D = output.Position;

	return output;
}

PixelToFrame ShadowMapPixelShaderFunction(ShadowMapVertexToPixel PSIn)
{
	PixelToFrame output = (PixelToFrame)0;

	output.Color = PSIn.Position2D.z / PSIn.Position2D.w;
	return output;
}

technique Shadow
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 ShadowMapVertexShader();
		PixelShader = compile ps_4_0 ShadowMapPixelShaderFunction();
	}
}

struct SSceneVertexToPixel
{
	float4 Position             : POSITION;
	float4 Pos2DAsSeenByLight    : TEXCOORD0;
};

struct SScenePixelToFrame
{
	float4 Color : COLOR0;
};

SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

	Output.Position = mul(inPos, WorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, LightsWorldViewProjection);
	return Output;
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

	Output.Color = tex2D(ShadowMapSampler, ProjectedTexCoords);

	return Output;
}
technique ShadowedScene
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 ShadowedSceneVertexShader();
		PixelShader = compile ps_4_0 ShadowedScenePixelShader();
	}
}