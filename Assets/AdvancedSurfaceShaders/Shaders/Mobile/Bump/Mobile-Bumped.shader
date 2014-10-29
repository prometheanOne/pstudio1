Shader "Advanced SS/Mobile/Bump/Diffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_BUMP

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert noforwardadd

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}