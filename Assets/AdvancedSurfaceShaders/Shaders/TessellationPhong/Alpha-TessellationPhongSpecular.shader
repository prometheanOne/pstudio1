Shader "Advanced SS/Tessellation (Phong)/Transparent/Specular"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _EdgeLength ("Edge length", Range(2,50)) = 5
        _Phong ("Phong Strength", Range(0,1)) = 0.54
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_TESSELLATIONPHONG

        #pragma target 5.0
        #include "Tessellation.cginc"
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong vertex:DispNone tessellate:TessEdge tessphong:_Phong alpha nolightmap

        ENDCG
    }

    Fallback "Specular"
}