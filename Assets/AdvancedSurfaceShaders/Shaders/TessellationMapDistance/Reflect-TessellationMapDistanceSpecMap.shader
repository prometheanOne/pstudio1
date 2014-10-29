Shader "Advanced SS/Tessellation Map (Distance)/Reflective/SpecMap"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _Tess ("Tessellation", Range(1,32)) = 4
        _TessDistanceMin ("Tessellation Min Distance", Float) = 10
        _TessDistanceMax ("Tessellation Max Distance", Float) = 25
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECMAP
        #define ADVANCEDSS_CUBEREFLECTION
        #define ADVANCEDSS_TESSELLATIONMAPDISTANCE

        #pragma target 5.0
        #include "Tessellation.cginc"
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong addshadow fullforwardshadows vertex:Disp tessellate:TessDistance nolightmap

        ENDCG
    }

    Fallback "Specular"
}