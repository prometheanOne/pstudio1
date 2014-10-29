Shader "Advanced SS/Triplanar/Triplanar (World)" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("X (RGB)", 2D) = "white" {}
		_MainTexY ("Y", 2D) = "white" {}
		_MainTexZ ("Z", 2D) = "white" {}
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert
        #if !SHADER_API_FLASH
        #pragma target 3.0
        #endif

		sampler2D _MainTex;
        sampler2D _MainTexY;
        sampler2D _MainTexZ;
        fixed4 _Color;
        half _Shininess;

		struct Input {
			float2 uv_MainTex;
            half3 wPosition;
            half3 wNormal;
		};

        void vert (inout appdata_full v, out Input o)
		{
            //o.localPosition = v.vertex.xyz;
            //o.worldNormal = v.normal;

            o.wPosition = mul((float4x4)_Object2World, float4(v.vertex.xyz, 1.0f)).xyz;
            //o.wNormal = mul((float4x4)_Object2World, float4(v.normal, 1.0f)).xyz;
            o.wNormal = mul( (float3x3) _Object2World ,v.normal );
        }

		void surf (Input IN, inout SurfaceOutput o)
        {
            //half3 localNormal = mul((float4x4)_World2Object, float4(IN.worldNormal, 1.0f));

            float3 blend_weights = abs( IN.wNormal );
            blend_weights = (blend_weights - 0.2) * 7; 
            blend_weights = max(blend_weights, 0);      // Force weights to sum to 1.0 (very important!) 
            blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z ).xxx;
            
            half4 col1 = tex2D (_MainTex, IN.wPosition.zy);
            half4 col2 = tex2D (_MainTexY, IN.wPosition.xz);
            half4 col3 = tex2D (_MainTexZ, IN.wPosition.xy);

            half4 blended_color = col1.xyzw * blend_weights.xxxx + 
            col2.xyzw * blend_weights.yyyy + 
            col3.xyzw * blend_weights.zzzz;

			o.Albedo = blended_color.rgb;
            o.Gloss = blended_color.a;
			o.Alpha = blended_color.a;
            o.Specular = _Shininess;
		}
		ENDCG
	} 
	FallBack "Specular"
}
