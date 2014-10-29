Shader "Advanced SS/Misc/RunningWater" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Water Texture", 2D) = "white" {}
        _TopTex ("Top Texture", 2D) = "white" {}
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _WaterSettings ("Tiling (XY) Speed (Z)", Vector) = (1,1,1,0)
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	    LOD 400
	    Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert alpha
        #pragma target 3.0

		sampler2D _MainTex;
        sampler2D _TopTex;
        float4 _WaterSettings;
        fixed4 _Color;
        half _Shininess;

		struct Input {
			float2 uv_MainTex;
            half3 wPosition;
            half3 wNormal;
            half2 waterMove;
		};

        void vert (inout appdata_full v, out Input o)
		{
            #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
            o.uv_MainTex = v.texcoord;
            #endif

            //o.localPosition = v.vertex.xyz;
            //o.worldNormal = v.normal;

            o.wPosition = mul((float4x4)_Object2World, float4(v.vertex.xyz, 1.0f)).xyz;
            //o.wNormal = mul((float4x4)_Object2World, float4(v.normal, 1.0f)).xyz;
            o.wNormal = mul( (float3x3) _Object2World ,v.normal );

            //float3 normal = normalize(v.normal);
            float3 tangent = normalize(cross(o.wNormal, float3(0,1,0)));
            float3 binormal = normalize(cross(tangent, o.wNormal));
            //o.waterMove = float2(_Time.y*tangent.y,_Time.y*binormal.y);
            o.waterMove = float2(0,_Time.y);
            //o.waterMove = float2(0,_Time.y*abs(o.wNormal.y));
        }

		void surf (Input IN, inout SurfaceOutput o)
        {
            //half3 localNormal = mul((float4x4)_World2Object, float4(IN.worldNormal, 1.0f));

            float3 blend_weights = abs( IN.wNormal );
            blend_weights = (blend_weights - 0.2) * 7; 
            blend_weights = max(blend_weights, 0);      // Force weights to sum to 1.0 (very important!) 
            blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z ).xxx;

            half4 waterBump1 = tex2D (_MainTex, IN.wPosition.zy * _WaterSettings.xy + half2(0,IN.waterMove.y * _WaterSettings.z));
            half4 waterBump2 = tex2D (_TopTex, IN.wPosition.xz * _WaterSettings.xy);
            half4 waterBump3 = tex2D (_MainTex, IN.wPosition.xy * _WaterSettings.xy + IN.waterMove * _WaterSettings.z);

            //half4 waterBump1 = tex2D (_MainTex, IN.wPosition.zy * _WaterSettings.xy + IN.waterMove * _WaterSettings.z);
            //half4 waterBump2 = tex2D (_MainTex, IN.wPosition.xz * _WaterSettings.xy);
            //half4 waterBump3 = tex2D (_MainTex, IN.wPosition.xy * _WaterSettings.xy + IN.waterMove * _WaterSettings.z);

            half4 blended_water = waterBump1 * blend_weights.xxxx + 
            waterBump2 * blend_weights.yyyy + 
            waterBump3 * blend_weights.zzzz;

			o.Albedo = blended_water.rgb * _Color.rgb;
            //o.Albedo = half3(IN.waterMove.x,0,0);
            //o.Normal = blended_water;
            o.Gloss = blended_water.a;
			o.Alpha = blended_water.a * _Color.a;
            o.Specular = _Shininess;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
