Shader "Advanced SS/Standard/GlossMap"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Texture", 2D) = "white" {}
        _SpecMap ("Gloss (R), Shininess (G)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_GLOSSMAP

        #pragma target 3.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong

        ENDCG
    }

    Fallback "Specular"
}