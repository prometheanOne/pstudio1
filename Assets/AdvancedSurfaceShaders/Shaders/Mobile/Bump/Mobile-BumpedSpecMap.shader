Shader "Advanced SS/Mobile/Bump/SpecMap"
{
    Properties
    {
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_SPECMAP
        #define ADVANCEDSS_BUMP

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}