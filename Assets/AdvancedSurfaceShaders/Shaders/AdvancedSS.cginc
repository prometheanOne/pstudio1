#ifndef ADVANCEDSS_CG_INCLUDED
#define ADVANCEDSS_CG_INCLUDED

#ifdef ADVANCEDSS_POM
    #ifndef ADVANCEDSS_PARALLAX
        #define ADVANCEDSS_PARALLAX
    #endif
#endif

#ifdef ADVANCEDSS_BUMP
    #define ADVANCEDSS_BUMP_OR_PARALLAX
#endif
#ifdef ADVANCEDSS_PARALLAX
    #define ADVANCEDSS_BUMP_OR_PARALLAX
    #define ADVANCEDSS_REQUIRESVIEWDIR
    #define ADVANCEDSS_REQUIRESSPECMAP
    #define ADVANCEDSS_PARALLAX_OR_RELIEF
#endif
#ifdef ADVANCEDSS_SPECULAR
    #define ADVANCEDSS_SPECULAR_OR_SPECMAP
#endif
#ifdef ADVANCEDSS_GLOSSMAP
    #ifndef ADVANCEDSS_SPECULAR_OR_SPECMAP
        #define ADVANCEDSS_SPECULAR_OR_SPECMAP
    #endif
    #ifndef ADVANCEDSS_REQUIRESSPECMAP
        #define ADVANCEDSS_REQUIRESSPECMAP
    #endif
#endif
#ifdef ADVANCEDSS_SPECMAP
    #ifndef ADVANCEDSS_SPECULAR_OR_SPECMAP
        #define ADVANCEDSS_SPECULAR_OR_SPECMAP
    #endif
    #ifndef ADVANCEDSS_REQUIRESSPECMAP
        #define ADVANCEDSS_REQUIRESSPECMAP
    #endif
#endif
#ifdef ADVANCEDSS_RIM
    #ifndef ADVANCEDSS_REQUIRESVIEWDIR
        #define ADVANCEDSS_REQUIRESVIEWDIR
    #endif
#endif
#ifdef ADVANCEDSS_RELIEF
    #ifndef ADVANCEDSS_REQUIRESVIEWDIR
        #define ADVANCEDSS_REQUIRESVIEWDIR
    #endif
    #ifndef ADVANCEDSS_PARALLAX_OR_RELIEF
        #define ADVANCEDSS_PARALLAX_OR_RELIEF
    #endif
#endif
#if defined(ADVANCEDSS_TESSELLATIONPHONG) || defined(ADVANCEDSS_TESSELLATIONMAPFIXED) || defined(ADVANCEDSS_TESSELLATIONMAPDISTANCE) || defined(ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)
    #define ADVANCEDSS_TESSELLATION
#endif
#if defined(ADVANCEDSS_TESSELLATIONMAPFIXED) || defined(ADVANCEDSS_TESSELLATIONMAPDISTANCE) || defined(ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)
    #ifndef ADVANCEDSS_REQUIRESSPECMAP
        #define ADVANCEDSS_REQUIRESSPECMAP
    #endif
#endif
#if defined(ADVANCEDSS_BUMP) || defined(ADVANCEDSS_PARALLAX) || defined(ADVANCEDSS_RELIEF)
    #define ADVANCEDSS_ANYBUMPEFFECT
#endif

struct Input
{
    float2 uv_MainTex;
#ifdef ADVANCEDSS_BUMP_OR_PARALLAX
    float2 uv_BumpMap;
#endif
#ifdef ADVANCEDSS_RELIEF
    float2 uv_BumpMap;
    float2 uv_RelaxedConeMap;
#endif
#ifdef ADVANCEDSS_REQUIRESVIEWDIR
    float3 viewDir;
#endif
#ifdef ADVANCEDSS_CUBEREFLECTION
	float3 worldRefl;
	INTERNAL_DATA
#endif
};

sampler2D _MainTex;
#if defined(ADVANCEDSS_BUMP_OR_PARALLAX) || defined(ADVANCEDSS_RELIEF)
sampler2D _BumpMap;
#endif
#ifdef ADVANCEDSS_REQUIRESSPECMAP
sampler2D _SpecMap;
#endif
#ifdef ADVANCEDSS_RELIEF
sampler2D _RelaxedConeMap;
#endif
#ifdef ADVANCEDSS_EMISSIVE
sampler2D _EmissiveMap;
half _EmissiveStrength;
#endif
#ifdef ADVANCEDSS_LIGHTINGRAMP
sampler2D _LightingRamp;
#endif
#ifndef ADVANCEDSS_MOBILE
fixed4 _Color;
#endif
#ifdef ADVANCEDSS_SPECULAR_OR_SPECMAP
half _Shininess;
#endif
#ifdef ADVANCEDSS_CUBEREFLECTION
fixed4 _ReflectColor;
samplerCUBE _Cube;
#endif
#ifdef ADVANCEDSS_PARALLAX_OR_RELIEF
float _Parallax;
#endif
#ifdef ADVANCEDSS_RIM
fixed4 _RimColor;
half _RimPower;
#endif
#ifdef ADVANCEDSS_POM
float _ParallaxSamples;
#endif
#ifdef ADVANCEDSS_RELIEF
fixed4 _ClipTiling;
#endif
#if defined(ADVANCEDSS_TESSELLATIONMAPFIXED) || defined(ADVANCEDSS_TESSELLATIONMAPDISTANCE)
float _Tess;
#endif
#ifdef ADVANCEDSS_TESSELLATIONMAPDISTANCE
float _TessDistanceMin;
float _TessDistanceMax;
#endif
#if defined(ADVANCEDSS_TESSELLATIONPHONG) || defined(ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)
float _EdgeLength;
#endif
#ifdef ADVANCEDSS_TESSELLATIONPHONG
float _Phong;
#endif
#if defined(ADVANCEDSS_TESSELLATIONMAPFIXED) || defined(ADVANCEDSS_TESSELLATIONMAPDISTANCE) || defined(ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)
float _Displacement;
#endif

#include "Assets/AdvancedSurfaceShaders/Shaders/AdvancedSSFunctions.cginc"
//#ifdef ADVANCEDSS_MOBILE
//#if defined(ADVANCEDSS_TESSELLATION) && defined(ADVANCEDSS_ANYBUMPEFFECT)
//#include "../../../AdvancedSSFunctions.cginc"
//#else
//#include "../../AdvancedSSFunctions.cginc"
//#endif
//#else
//#if defined(ADVANCEDSS_TESSELLATION) && defined(ADVANCEDSS_ANYBUMPEFFECT)
//#include "../../AdvancedSSFunctions.cginc"
//#else
//#include "../AdvancedSSFunctions.cginc"
//#endif
//#endif

//typedef string fff = "../AdvancedSSFunctions.cginc";
//#include "fff"

//void advancedVertexShader (inout appdata_full v, out Input o)
//{
//}

void advancedSurfaceShader (Input IN, inout SurfaceOutput o)
{
#ifdef ADVANCEDSS_PARALLAX
    #ifdef ADVANCEDSS_POM
    float2 offset = ParallaxOcclusionOffset( IN.viewDir, _Parallax, o.Normal, IN.uv_BumpMap, _SpecMap, _ParallaxSamples );
    IN.uv_MainTex -= offset;
    IN.uv_BumpMap -= offset;
    #else
    half height = tex2D (_SpecMap, IN.uv_BumpMap).w;
    float2 offset = ParallaxOffset (height, _Parallax, IN.viewDir);
    IN.uv_MainTex += offset;
    IN.uv_BumpMap += offset;
    #endif
#endif
#ifdef ADVANCEDSS_RELIEF
    float2 rcs = RelaxedConeStep( IN.viewDir, _Parallax, IN.uv_RelaxedConeMap, _RelaxedConeMap, _ClipTiling );
    float2 offset = rcs - IN.uv_RelaxedConeMap;
    IN.uv_MainTex += offset;
    IN.uv_BumpMap += offset;
#endif

    half4 tex = tex2D(_MainTex, IN.uv_MainTex);
#ifndef ADVANCEDSS_MOBILE
    o.Albedo = tex.rgb * _Color.rgb;
#else
    o.Albedo = tex.rgb;
#endif

#ifdef ADVANCEDSS_GLOSSMAP
    half3 specMapCol = tex2D(_SpecMap, IN.uv_MainTex).rgb;
    o.Gloss = specMapCol.r;
    o.Specular = specMapCol.g;
#endif
#ifdef ADVANCEDSS_SPECMAP
    half3 specMapCol = tex2D(_SpecMap, IN.uv_MainTex).rgb;
    o.Gloss = Luminance(specMapCol);
    _SpecColor *= float4(specMapCol.r, specMapCol.g, specMapCol.b, 1);
    o.Specular = _Shininess;
#endif
#ifdef ADVANCEDSS_SPECULAR
    o.Gloss = tex.a;
    o.Specular = _Shininess;
#endif

#ifndef ADVANCEDSS_MOBILE
    o.Alpha = tex.a * _Color.a;
#else
    o.Alpha = tex.a;
#endif

#ifdef ADVANCEDSS_BUMP_OR_PARALLAX
    o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
#endif
#ifdef ADVANCEDSS_RELIEF
    //o.Normal = ConeStepNormal(tex2D(_BumpMap, rcs).rgb);
    o.Normal = UnpackNormal (tex2D (_BumpMap, rcs));
#endif

#ifdef ADVANCEDSS_CUBEREFLECTION
    float3 worldRefl = WorldReflectionVector (IN, o.Normal);
	fixed4 reflcol = texCUBE (_Cube, worldRefl);
	reflcol *= tex.a;
	o.Emission = reflcol.rgb * _ReflectColor.rgb * _ReflectColor.a;
#endif

#ifdef ADVANCEDSS_RIM
    half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    #ifdef ADVANCEDSS_EMISSIVE
    o.Emission += (tex2D(_EmissiveMap, IN.uv_MainTex).rgb * _EmissiveStrength) + (_RimColor.rgb * pow (rim, _RimPower));
    #else
    o.Emission += _RimColor.rgb * pow (rim, _RimPower);
    //o.Emission = IN.viewDir;
    #endif
#else
    #ifdef ADVANCEDSS_EMISSIVE
    o.Emission += (tex2D(_EmissiveMap, IN.uv_MainTex).rgb * _EmissiveStrength);
    #endif
#endif
}

#endif
