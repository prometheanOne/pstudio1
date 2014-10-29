Shader "Advanced SS/Mobile/Standard/GlossMap Emissive Ramp"
{
    Properties
    {
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _EmissiveStrength ("Emissive Strength", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
        _SpecMap ("Gloss (R), Shininess (G)", 2D) = "white" {}
        _EmissiveMap ("EmissiveMap (RGB)", 2D) = "black" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_GLOSSMAP
        #define ADVANCEDSS_EMISSIVE
        #define ADVANCEDSS_LIGHTINGRAMP

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader MobileBlinnPhongRamp exclude_path:prepass nolightmap noforwardadd halfasview

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}