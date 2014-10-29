Shader "Advanced SS/Tessellation Map (Fixed)/Diffuse"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Tess ("Tessellation", Range(1,32)) = 4
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _MainTex ("Texture", 2D) = "white" {}
        _SpecMap ("Heightmap (A)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_TESSELLATIONMAPFIXED

        #pragma target 5.0
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert addshadow fullforwardshadows vertex:Disp tessellate:TessFixed nolightmap

        ENDCG
    }

    Fallback "Diffuse"
}