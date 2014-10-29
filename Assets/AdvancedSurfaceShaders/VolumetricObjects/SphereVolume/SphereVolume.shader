Shader "Advanced SS/Volumetric/Sphere Volume"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RadiusSqr ("Radius", Float) = 1.0
        _TextureData ("Texture Speed (X,Y,Z), Texture Scale (W)", Vector) = (10,-0.1,0,1)
        _Visibility ("Visibility", Float) = 1.0
        _BIsForwardLighting ("Is Forward Lighting", Float) = 0
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay-1" "IgnoreProjector"="True" "RenderType"="Transparent" }
        //Blend Off                           // No Blend
        Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
        //Blend One One                       // Additive
        //Blend One OneMinusDstColor          // Soft Additive
        //Blend DstColor Zero                 // Multiplicative
        //Blend DstColor SrcColor             // 2x Multiplicative

        Cull Front
        Lighting Off
        ZWrite Off
        ZTest Always
        //Fog { Color (0,0,0,0) }
        LOD 200
	
        Pass
        {	
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 3.0
            //#pragma only_renderers d3d9
            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _Color;

            #define SAMPLE_METHOD

            //==============================
            // SPHERE INTERSECTION
            //==============================
            inline float IntersectSphere(float3 rayOrigin, float3 rayDirection, float radius, inout float t1, inout float t2)
            {
                // geometric solution
                float tca = dot(rayOrigin, -rayDirection);
                //if (tca < 0) { return 0.0f; }
                float d2 = dot(rayOrigin, rayOrigin) - tca * tca;
                if (d2 > radius) { return 0.0f; }
                float thc = sqrt(radius - d2);
                t2 = tca - thc;
                t1 = tca + thc;

                return 1.0f;
            }

            struct VSOutput
            {
                float4 vPos         : SV_POSITION;
                float3 vWorldPos    : TEXCOORD0;
                float4 vScreenPos   : TEXCOORD1;
                float3 vLocalPos    : TEXCOORD2;
                float3 vViewPos     : TEXCOORD3;
                float3 vLocalCamPos : TEXCOORD4; // constant not available in pixel shader so have to send it through here
            };

            float4 _TextureData;
            half _RadiusSqr;
            float _Visibility;
            float _BIsForwardLighting;

            VSOutput vert (appdata_full IN)
            {
                VSOutput OUT;

                OUT.vLocalPos = IN.vertex.xyz;
                OUT.vWorldPos.xyz = mul((float4x4)_Object2World, float4(IN.vertex.xyz, 1.0f)).xyz;
                OUT.vViewPos = mul((float4x4)UNITY_MATRIX_MV, float4(IN.vertex.xyz, 1.0f)).xyz;
                OUT.vLocalCamPos = mul((float4x4)_World2Object, (float4(_WorldSpaceCameraPos, 1.0f))).xyz;
                OUT.vPos = mul(UNITY_MATRIX_MVP, IN.vertex);
                OUT.vScreenPos = ComputeScreenPos(OUT.vPos);

                return OUT;
            }

            float4 frag (VSOutput IN) : COLOR
            {
                float3 eyeDirectionWorld = normalize(IN.vWorldPos - _WorldSpaceCameraPos);
                float3 eyeDirectionLocal = normalize(IN.vLocalPos - IN.vLocalCamPos);
                //float3 eyeVecLocal = -eyeDirectionLocal;

                //return float4(IN.vLocalPos.xyz / (_BoxMax*2), 1);

                float3 rayOrigin = IN.vLocalCamPos;
                float3 rayDirection = eyeDirectionLocal;
                float t1, t2;

                float bValidIntersection = IntersectSphere(rayOrigin, rayDirection, _RadiusSqr, t1, t2);

                t1 *= step(0, t1);
                t2 *= step(0, t2);

                // Get distance from eye to depth buffer fragment
                float2 screenUV = IN.vScreenPos.xy/IN.vScreenPos.w;

                float4 depthTexture = tex2D(_CameraDepthTexture, screenUV);
                float uniformDistance = DECODE_EYEDEPTH(depthTexture.r);
                float3 viewEyeDirection = normalize(IN.vViewPos);
                float scaleFactor = (uniformDistance / viewEyeDirection.z);
                float distanceToDepthFragment = length(viewEyeDirection * scaleFactor);

                // Calculate new t1/t2 using depth buffer
                float tFar = (max(t1, t2));
                float tNear = (min(t1, t2));

                // Calculate amount of volume being blocked by depth buffer
                tFar = min(tFar, distanceToDepthFragment);
                tNear = min(tNear, distanceToDepthFragment);

#if defined(SAMPLE_METHOD)
                float intensity = 0;
                int sampleCount = 20;
                float invSampleCount = 1.0f/(float)sampleCount;
                float sampleRange = tFar - tNear;
                float sampleStep = sampleRange * invSampleCount;
                float totalUniformIntensity = pow(saturate(sampleRange / _Visibility), 1);
                for(int s=0; s<sampleCount; s++)
                {
                    float tSample = tNear + (sampleStep * (float)s);
                    float3 samplePosWorld = _WorldSpaceCameraPos + (eyeDirectionWorld * tSample);
                    float3 samplePosLocal = IN.vLocalCamPos + (eyeDirectionLocal * tSample);
          
                    // Texture
                    half phaseX = _Time.y * _TextureData.x;
                    half phaseY = _Time.y * _TextureData.y;
                    half phaseZ = _Time.y * _TextureData.z;
                    float2 xyOffset = float2(phaseX, phaseY);
                    float2 zyOffset = float2(phaseZ, phaseY);
                    float scale = _TextureData.w;
                    float noiseValue = tex2D(_MainTex, samplePosWorld.xy*scale + xyOffset).r + tex2D(_MainTex, samplePosWorld.zy*scale + zyOffset).r;

                    // Calculate distance attenuation
                    //float distanceAttenuation = 1.0f - saturate(tSample / 50.0f);

                    // Calculate sample intensity
                    intensity += (invSampleCount * noiseValue);// * distanceAttenuation;
                }

                intensity *= totalUniformIntensity;// * cap;//clamp(-IN.vLocalPos.y*10-(_StartFade*10-1),minClamp,1);// * clamp(pow(falloffRange,_FalloffPower)*_Falloff,1,1000000);
#else // (!)defined(SAMPLE_METHOD)
                float volumeAmount = tFar - tNear;

                float intensity = saturate(volumeAmount / _Visibility);
#endif // defined(SAMPLE_METHOD)

                return float4(_Color.rgb, intensity * _Color.a);
            }

		ENDCG
        }
	} 
}
