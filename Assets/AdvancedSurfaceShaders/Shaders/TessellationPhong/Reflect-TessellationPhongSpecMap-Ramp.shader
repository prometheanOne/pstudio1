Shader "Advanced SS/Tessellation (Phong)/Reflective/SpecMap Ramp"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _EdgeLength ("Edge length", Range(2,50)) = 5
        _Phong ("Phong Strength", Range(0,1)) = 0.54
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECMAP
        #define ADVANCEDSS_CUBEREFLECTION
        #define ADVANCEDSS_LIGHTINGRAMP
        #define ADVANCEDSS_TESSELLATIONPHONG

        #pragma target 5.0
        #include "Tessellation.cginc"
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhongRamp vertex:DispNone tessellate:TessEdge tessphong:_Phong nolightmap

        ENDCG
    }

    Fallback "Specular"
}