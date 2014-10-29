Shader "Advanced SS/Standard/Reflective/Diffuse"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_CUBEREFLECTION

        #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
        #pragma target 5.0
        #else
        #pragma target 3.0
        #endif
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader Lambert

        ENDCG
    }

    Fallback "Diffuse"
}