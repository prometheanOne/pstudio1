Shader "Advanced SS/Standard/Diffuse Rim Ramp"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (0.75,0.75,0.75,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _MainTex ("Texture", 2D) = "white" {}
        _LightingRamp ("Lighting Ramp (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_RIM
        #define ADVANCEDSS_LIGHTINGRAMP

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader LambertRamp

        ENDCG
    }

    Fallback "Diffuse"
}