
float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;
float3 CameraPosition;

// Ambient 
bool ambientEnabled = false;
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

// Speculare 
bool specularEnabled = false;
float Shininess = 400;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = .6;
float3 SpecularDirection = float3(1, 1, 0);

// Direction 
bool directionLightEnabled = false;
float3 DiffuseLightDirection = float3(1, 1, 0);
float4 DiffuseColor = float4(0, 1, 0, 1); //kan trixa med detta
float DiffuseIntensity = 0.1;



// Texture mapping
bool textureMappingEnabled = false;
texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Mirror;
	AddressV = Mirror;
};

// Bump mapping
bool bumpMappingEnabled = false;
float BumpConstant = 1;
texture NormalMap;

sampler2D bumpSampler = sampler_state {
	Texture = (NormalMap);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

//Transparency
bool TransparencyEnabled = false;
float Transparency = 0.5;
float TransparencyThreshold = 0.1;

//Fog
bool FogEnabled = false;
float FogStart;
float FogEnd;
float4 FogColor = float4(1, 0, 0, 1);

// Reflection
bool ReflectionEnabled = false;
float ReflectionAmount;
float4 TintColor = float4(1, 1, 1, 1);
Texture EnvironmentMapTexture;

samplerCUBE EnvironmentSampler = sampler_state
{
	texture = <EnvironmentMapTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput
{
	float3 Binormal : BINORMAL0;
	float4 Normal : NORMAL0;
	float4 Position : POSITION0;
	float3 Tangent : TANGENT0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float3 Binormal : BINORMAL0;
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float3 Tangent : TANGENT0;
	float2 TextureCoordinate : TEXCOORD1;
	float3 Reflection : TEXCOORD2;
	float Fog : FOG;
	float3 ViewVector : POSITION1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	//float3 viewVector = worldPosition - CameraPosition;

	float3 viewVector = CameraPosition - worldPosition;
	output.ViewVector = normalize(viewVector);
	output.Position = mul(viewPosition, Projection);
	output.TextureCoordinate = input.TextureCoordinate;
	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
	output.Tangent = normalize(mul(input.Tangent, WorldInverseTranspose));
	output.Binormal = normalize(mul(input.Binormal, WorldInverseTranspose));

	//så kan man inne i programmet bara sätta true/false
	if (FogEnabled) {
		float distanceToCamera = length(worldPosition - CameraPosition);
		output.Fog = saturate((distanceToCamera - FogStart) / (FogEnd - FogStart));
	}
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
float4 returnColor = { 1,1,1,1 };
float4 textureColor = { 0,0,0,0 };
float4 lights = { 0,0,0,0 };
float3 normal;

if (bumpMappingEnabled)
{
	float3 bump = BumpConstant * (tex2D(bumpSampler, input.TextureCoordinate) - (0.5, 0.5, 0.5));
	float3 bumpNormal = input.Normal + (bump.x * input.Tangent + bump.y * input.Binormal);
	bumpNormal = normalize(bumpNormal);
	normal = bumpNormal;
}
else {
	normal = normalize(input.Normal);
}


if (textureMappingEnabled)
{
	textureColor = tex2D(textureSampler, input.TextureCoordinate);
}

if (ambientEnabled)
{
	if (textureMappingEnabled)
		lights += AmbientColor * AmbientIntensity * textureColor;
	else
		lights += AmbientColor * AmbientIntensity;
}
if (directionLightEnabled)
{
	float nl = max(0, dot(normalize(DiffuseLightDirection),normal));
	lights += DiffuseIntensity * DiffuseColor * nl;
}
if (specularEnabled) {

	float3 light = normalize(SpecularDirection);
	float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(input.ViewVector);

	float dotProduct = dot(r, v);
	float4 specular = SpecularIntensity * SpecularColor * max(pow(abs(dotProduct), Shininess), 0);

	lights += specular;
}

if (ReflectionEnabled) {

	float3 reflections = normalize(reflect(input.ViewVector, normal));
	reflections.x = -reflections.x;
	reflections.y = -reflections.y;
	float4 color = TintColor * texCUBE(EnvironmentSampler, reflections * ReflectionAmount);
	returnColor += color;
}

returnColor = saturate(returnColor * lights);

if (TransparencyEnabled) {
	clip(Transparency < TransparencyThreshold ? -1 : 1);
}
else {
	Transparency = 1;
}
returnColor.a = Transparency;

if (FogEnabled) {
	return lerp(returnColor, FogColor, input.Fog);
}
return returnColor;
}

technique All
{
	pass P0
	{
		VertexShader = compile vs_4_0  VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
};
