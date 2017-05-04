#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define SHADER_MAX_BONES 50
float4x4 matBones[SHADER_MAX_BONES];

///Matrices
matrix World;
matrix View;
matrix ViewInverse;
matrix WorldView;
matrix worldViewProjection;
//Materials
float3 diffuseColor;
float3 specularColor;
float specularPower;
//Light
float3 ambientLightColor;
float3 light1Position;
float3 light1Color;
float3 light2Position;
float3 light2Color;
//Textures
float2 uvoTile;
texture diffuseTexture : Diffuse;

sampler2D diffuseSampler = sampler_state
{
    texture = <diffuseTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
};

struct VertexShaderInput
{
	float4 Position : POSITION;
    float3 Normal : NORMAL;
    float2 Uv0 : TEXCOORD0;
    float4 BoneIndex : BLENDINDICES0;
    float4 BoneWeight : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
	float4 hpPosition : POSITION;
    float2 Uv0 : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 lightVec1 : TEXCOORD2;
    float3 lightVec2 : TEXCOORD3;
    float3 eyeVec : TEXCOORD4;

};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    
    matrix matTransform = (float1x1) matBones[input.BoneIndex.x] * input.BoneWeight.x;
    matTransform += (float1x1) matBones[input.BoneIndex.y] * input.BoneWeight.y;
    matTransform += (float1x1) matBones[input.BoneIndex.z] * input.BoneWeight.z;
    float finalWeight = 1.0f - (input.BoneWeight.x + input.BoneWeight.y + input.BoneWeight.z);
    matTransform += matBones[input.BoneIndex.w] * finalWeight;

    // Transform vertex and normal
    float4 position = mul(input.Position, matTransform);
    float3 normal = mul(input.Normal, (float3x4)matTransform);

    output.hpPosition = mul(position, worldViewProjection);
    output.Normal = mul(normal, (float3x4)WorldView);

    // Calculate light and eye vectors
    float4 worldPosition = mul(position, World);
    output.eyeVec = mul(ViewInverse[3].xyz - worldPosition.xyz, (float3x4)View);
    output.lightVec1 = mul(light1Position - worldPosition.xyz, (float3x4)View);
    output.lightVec2 = mul(light2Position - worldPosition.xyz, (float3x4)View);
    output.Uv0 = input.Uv0;

	return output;
}

void phongShading(in float3 normal, in float3 lightVec, in float3 halfwayVec, in float3 lightColor, out float3 diffuseColor, out float3 specularColor)
{
    float diffuseInt = saturate(dot(normal, lightVec));
    diffuseColor = diffuseInt * lightColor;
    float specularInt = saturate(dot(normal, halfwayVec));
    specularInt = pow(specularInt, specularPower);
    specularColor = specularInt * lightColor;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Normalize all input vectors
    float3 normal = normalize(input.Normal);
    float3 eyeVec = normalize(input.eyeVec);
    float3 lightVec1 = normalize(input.lightVec1);
    float3 lightVec2 = normalize(input.lightVec2);
    float3 halfwayVec1 = normalize(lightVec1 + eyeVec);
    float3 halfwayVec2 = normalize(lightVec2 + eyeVec);

    // Calculate diffuse and specular color for each light
    float3 diffuseColor1, diffuseColor2;
    float3 specularColor1, specularColor2;
    phongShading(normal, lightVec1, halfwayVec1, light1Color, diffuseColor1, specularColor1);
    phongShading(normal, lightVec2, halfwayVec2, light2Color, diffuseColor2, specularColor2);

    float4 materialColor = tex2D(diffuseSampler, input.Uv0);

    // calculate the final color of each pixel, combining its color with the diffuse and specular components from the light.

    float4 finalColor;
    finalColor.a = 1.0f;
    finalColor.rgb = materialColor.xyz * ((diffuseColor1 + diffuseColor2) * diffuseColor + ambientLightColor) 
    + (specularColor1 + specularColor2) * specularColor;

	return finalColor;
}


technique AnimatedModelDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

