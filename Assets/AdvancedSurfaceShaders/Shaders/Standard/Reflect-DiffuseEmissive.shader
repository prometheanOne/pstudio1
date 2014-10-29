Shader "Advanced SS/Standard/Reflective/Diffuse Emissive"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _EmissiveStrength ("Emissive Strength", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _EmissiveMap ("EmissiveMap (RGB)", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_EMISSIVE
        #define ADVANCEDSS_CUBEREFLECTION

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert

        ENDCG
    }

    Fallback "Diffuse"
}