Shader "Advanced SS/Bump/Reflective/SpecMap Emissive"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _EmissiveStrength ("Emissive Strength", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
        _EmissiveMap ("EmissiveMap (RGB)", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECMAP
        #define ADVANCEDSS_BUMP
        #define ADVANCEDSS_EMISSIVE
        #define ADVANCEDSS_CUBEREFLECTION

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong

        ENDCG
    }

    Fallback "Specular"
}