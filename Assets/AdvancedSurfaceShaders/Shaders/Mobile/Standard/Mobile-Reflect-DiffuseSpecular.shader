Shader "Advanced SS/Mobile/Standard/Reflective/Specular"
{
    Properties
    {
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque"}
        LOD 500

        CGPROGRAM

        #define ADVANCEDSS_MOBILE
        #define ADVANCEDSS_SPECULAR
        #define ADVANCEDSS_CUBEREFLECTION

        #include "../../AdvancedSS.cginc"
        #pragma surface advancedSurfaceShader MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview

        ENDCG
    }

    Fallback "Mobile/VertexLit"
}