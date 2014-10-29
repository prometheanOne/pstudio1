Shader "Advanced SS/Mobile/Standard/SpecMap"
{
    Properties
    {
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Texture", 2D) = "white" {}
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_SPECMAP

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}