Shader "Advanced SS/Tessellation Map (Edge Length)/Transparent/Cutout/Reflective/Specular"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _EdgeLength ("Edge length", Range(2,50)) = 5
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _SpecMap ("Heightmap (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_CUBEREFLECTION
        #define ADVANCEDSS_TESSELLATIONMAPEDGELENGTH

        #pragma target 5.0
        #include "Tessellation.cginc"
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong addshadow fullforwardshadows vertex:Disp tessellate:TessEdge alphatest:_Cutoff nolightmap

        ENDCG
    }

    Fallback "Specular"
}