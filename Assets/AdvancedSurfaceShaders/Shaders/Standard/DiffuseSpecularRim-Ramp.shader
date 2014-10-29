Shader "Advanced SS/Standard/Specular Rim Ramp"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
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

        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_RIM
        #define ADVANCEDSS_LIGHTINGRAMP

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhongRamp

        ENDCG
    }

    Fallback "Specular"
}