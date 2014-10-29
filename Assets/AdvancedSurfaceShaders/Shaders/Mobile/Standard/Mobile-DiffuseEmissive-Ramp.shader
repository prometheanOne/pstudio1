Shader "Advanced SS/Mobile/Standard/Diffuse Emissive Ramp"
{
    Properties
    {
        _EmissiveStrength ("Emissive Strength", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
        _EmissiveMap ("EmissiveMap (RGB)", 2D) = "black" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_EMISSIVE
        #define ADVANCEDSS_LIGHTINGRAMP

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader LambertRamp noforwardadd

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}