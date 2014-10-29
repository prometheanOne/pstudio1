Shader "Advanced SS/Standard/Diffuse Ramp"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_LIGHTINGRAMP

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader LambertRamp

        ENDCG
    }

    Fallback "Diffuse"
}