Shader "Advanced SS/Triplanar/Triplanar Bumped (World) Rim" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (0.75,0.75,0.75,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_MainTex ("Diffuse X (RGB)", 2D) = "white" {}
		_MainTexY ("Diffuse Y", 2D) = "white" {}
		_MainTexZ ("Diffuse Z", 2D) = "white" {}
        _BumpMap ("Bumpmap X", 2D) = "bump" {}
		_BumpMapY ("Bumpmap Y", 2D) = "bump" {}
		_BumpMapZ ("Bumpmap Z", 2D) = "bump" {}
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert
        #pragma target 3.0

		sampler2D _MainTex;
        sampler2D _MainTexY;
        sampler2D _MainTexZ;
		sampler2D _BumpMap;
        sampler2D _BumpMapY;
        sampler2D _BumpMapZ;
        fixed4 _Color;
        half _Shininess;
        fixed4 _RimColor;
        half _RimPower;

		struct Input {
			float2 uv_MainTex;
            half3 wPosition;
            half3 wNormal;
            float3 viewDir;
		};

        void vert (inout appdata_full v, out Input o)
		{
            o.wPosition = mul((float4x4)_Object2World, float4(v.vertex.xyz, 1.0f)).xyz;
            o.wNormal = mul( (float3x3) _Object2World ,v.normal );
        }

		void surf (Input IN, inout SurfaceOutput o)
        {
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
            
            half3 n1 = UnpackNormal(tex2D (_BumpMap, IN.wPosition.zy));
            half3 n2 = UnpackNormal(tex2D (_BumpMapY, IN.wPosition.xz));
            half3 n3 = UnpackNormal(tex2D (_BumpMapZ, IN.wPosition.xy));

            half3 blended_normal = n1.xyz * blend_weights.xxx + 
            n2.xyz * blend_weights.yyy + 
            n3.xyz * blend_weights.zzz;

			o.Albedo = blended_color.rgb;
            o.Normal = blended_normal;
            o.Gloss = blended_color.a;
			o.Alpha = blended_color.a;
            o.Specular = _Shininess;
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), normalize(o.Normal)));
            o.Emission = _RimColor.rgb * pow (rim, _RimPower);
		}
		ENDCG
	} 
	FallBack "Specular"
}
