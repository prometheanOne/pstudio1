Shader "Advanced SS/Mobile/Bump/Specular Rim"
{
    Properties
    {
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _RimColor ("Rim Color", Color) = (0.75,0.75,0.75,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_BUMP
        #define ADVANCEDSS_RIM

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}