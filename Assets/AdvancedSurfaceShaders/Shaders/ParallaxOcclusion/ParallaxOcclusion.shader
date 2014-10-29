Shader "Advanced SS/Parallax Occlusion (D3D)/Diffuse"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Parallax ("Height", Range (0.005, 0.08)) = 0.08
        _ParallaxSamples ("Parallax Samples", Range (10, 50)) = 40
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _SpecMap ("Heightmap (A)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_POM

        #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
        #pragma target 5.0
        #else
        #pragma target 3.0
        #endif
        #pragma only_renderers d3d9 d3d11 d3d11_9x
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert

        ENDCG
    }

    Fallback "Diffuse"
}