Shader "Advanced SS/Relaxed Cone Stepping/Reflective/SpecMap"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _Parallax ("Height", Range (0.005, 0.08)) = 0.08
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _SpecMap ("SpecMap (RGB)", 2D) = "white" {}
        _RelaxedConeMap ("RelaxedConeMap", 2D) = "white" {}
        _ClipTiling ("Relief Clip Tiling U,V", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECMAP
        #define ADVANCEDSS_RELIEF
        #define ADVANCEDSS_CUBEREFLECTION

        #pragma exclude_renderers d3d11_9x
        #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
        #pragma target 5.0
        #else
        #pragma target 3.0
        #endif
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong

        ENDCG
    }

    Fallback "Specular"
}