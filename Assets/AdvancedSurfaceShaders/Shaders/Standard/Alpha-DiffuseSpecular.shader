Shader "Advanced SS/Standard/Transparent/Specular"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_SPECULAR

        #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
        #pragma target 5.0
        #else
        #pragma target 3.0
        #endif
        #include "../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader BlinnPhong alpha

        ENDCG
    }

    Fallback "Specular"
}