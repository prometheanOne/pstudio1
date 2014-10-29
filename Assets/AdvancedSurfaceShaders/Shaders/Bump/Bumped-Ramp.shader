Shader "Advanced SS/Bump/Diffuse Ramp"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_BUMP
        #define ADVANCEDSS_LIGHTINGRAMP

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader LambertRamp

        ENDCG
    }

    Fallback "Diffuse"
}