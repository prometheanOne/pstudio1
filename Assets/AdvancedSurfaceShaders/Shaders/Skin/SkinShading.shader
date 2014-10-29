Shader "Advanced SS/Skin/Skin Shading" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Kspec ("Ks", Float) = 0.45
		_Kdiffuse ("Kd", Float) = 1
		_Kambient ("Ka", Float) = 0.2
		_ColourIntensity ("ColourIntensity", Range(1, 2.2)) = 1.69
		_ShadowStrength ("ShadowStrength", Range (0, 1)) = 0.94
		_NormalBlend ("NormalBlend", Range (0, 2)) = 1.22
		_RedWeight ("RedWeight", Range (-0.1, 0.3)) = 0.0
		_Shininess ("Shininess", Range (0.01, 1)) = 0.74
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf SSS fullforwardshadows vertex:vert
		#pragma target 3.0
		#include "UnityCG.cginc"
		
		struct SurfaceOutputSSS { 
			fixed3 Albedo; 
			fixed3 Normal;
			fixed3 VertNormal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		}; 
		
		float _Kspec;
		float _Kdiffuse;
		float _Kambient;
		float _ColourIntensity;
		float _ShadowStrength;
		float _RedWeight;
		float _NormalBlend;
		float _Shininess;
		
		float3 BlendNormals(float lightDiffusion, float vertexNdotL, float bumpNdotL)
		{
			// Tweak max values as you see fit.
			float redIntensity  = lerp(0.0f, 0.5f, lightDiffusion);
			float greenBlueIntensity = 1;//lerp(0.6f, 1.0f, lightDiffusion);

			float red = lerp(vertexNdotL, bumpNdotL, redIntensity);
			float greenBlue = lerp(vertexNdotL, bumpNdotL, greenBlueIntensity);
			greenBlue = min(red, greenBlue); // remove unwanted green/blue

			// Put it all together.
			//float3 result = float3(red, greenBlue, greenBlue);
			return saturate(float3(red, greenBlue, greenBlue));
		}

		half4 LightingSSS (SurfaceOutputSSS s, half3 lightDir, half3 viewDir, half atten) {
		
			#ifndef USING_DIRECTIONAL_LIGHT 
			lightDir = normalize(lightDir).xyz; 
			#endif
			viewDir = normalize(viewDir).xyz;
		    
			float4 colourWeights = float4( 0.4 + _RedWeight, 0.4, 0.4, 1 );
			
			float3 diffuse = BlendNormals(_NormalBlend, dot(s.VertNormal, lightDir), dot(s.Normal, lightDir) );
			//float diffuseLum = Luminance(diffuse);
			//float finalShadow = 
			
			float3 realModelColour = pow( s.Albedo.rgb, _ColourIntensity );
			float3 linearLightColour = pow(_LightColor0.rgb, _ColourIntensity) * atten;
			//float3 linearLightColour = _LightColor0.rgb, _ColourIntensity * atten;
			
			float3 diffusePoint = _Kdiffuse * linearLightColour * diffuse;
			
			float3 diffuseBlurred = _Kdiffuse * max(diffuse, (1-_ShadowStrength));
			float3 diffuseColour = colourWeights.xyz * diffusePoint + lerp((float3(1,1,1) - colourWeights.xyz) * diffusePoint, diffuseBlurred, saturate(linearLightColour).rgb );
			
			half3 h = normalize (lightDir.xyz + viewDir.xyz).xyz;
			float nh = max (0, dot (s.Normal, h));
			//float3 specular = specularLight += lightColor[i] * lightShadow[i] * rho_s * specBRDF( N, V, L[i], eta, m) * saturate( dot( N, L[i] ) );  
			float3 specular = pow (nh, s.Specular*128.0) * linearLightColour;
			
			half4 c;
			c.rgb = _Kambient * diffuse * (realModelColour*linearLightColour) + (diffuseColour * realModelColour + _Kspec * linearLightColour * specular );
		
			//c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
			//c.a = s.Alpha;
			c.a = 0;//s.Alpha + _LightColor0.a * specular * atten; 
			return c;
			//return float4(linearLightColour, linearLightColour, linearLightColour, 1);
		}

        float4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;

        //#ifdef SHADER_API_D3D11
        //uniform float4 _MainTex_ST;
        //uniform float4 _BumpMap_ST;
        //#endif

		struct Input
        {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			half3 VertNormal;
		};

        void vert (inout appdata_full v, out Input o)
        {
            //o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
            //o.uv_BumpMap = TRANSFORM_TEX(v.texcoord, _BumpMap);

            #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
            o.uv_MainTex = v.texcoord;
            o.uv_BumpMap = v.texcoord;
            #endif

            float3 binormal = cross( v.normal, v.tangent.xyz ).xyz * v.tangent.w;
	        float3x3 rotation = float3x3( v.tangent.xyz, binormal, v.normal );
            o.VertNormal = mul(rotation,v.normal.xyz).xyz;
        }

		void surf (Input IN, inout SurfaceOutputSSS o)
        {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.VertNormal = IN.VertNormal;
		    o.Specular = _Shininess;
			o.Alpha = c.a * _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
