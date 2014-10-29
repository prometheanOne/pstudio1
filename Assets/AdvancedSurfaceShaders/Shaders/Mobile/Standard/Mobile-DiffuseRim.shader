Shader "Advanced SS/Mobile/Standard/Diffuse Rim"
{
    Properties
    {
        _RimColor ("Rim Color", Color) = (0.75,0.75,0.75,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_RIM

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert noforwardadd

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}