Shader "Advanced SS/Tessellation Map (Edge Length)/Transparent/Specular"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _EdgeLength ("Edge length", Range(2,50)) = 5
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _MainTex ("Texture", 2D) = "white" {}
        _SpecMap ("Heightmap (A)", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_TESSELLATIONMAPEDGELENGTH

        #pragma target 5.0
        #include "Tessellation.cginc"
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong addshadow fullforwardshadows vertex:Disp tessellate:TessEdge alpha nolightmap

        ENDCG
    }

    Fallback "Specular"
}