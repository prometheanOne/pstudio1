Shader "Hidden/AdvancedSS/DrawDepth" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
		
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		float4 depth : TEXCOORD0;
	};
		
	sampler2D _MainTex;
	sampler2D _CameraDepthTexture;
		
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
        //float4 depth = ComputeScreenPos (o.pos);
        //o.sPos = ComputeScreenPos(o.pos);
        //COMPUTE_EYEDEPTH(o.sPos.z);
        o.depth = EncodeFloatRGBA(1 - COMPUTE_DEPTH_01);
        //o.uv = float2(1 - depth.z, 0);
		//o.uv =  v.texcoord.xy;
		return o;
	}
	
	half4 frag(v2f i) : COLOR 
	{
		//float d = UNITY_SAMPLE_DEPTH( tex2D(_CameraDepthTexture, i.uv.xy) );
		//d = 1-Linear01Depth(d);
        //float d = Linear01Depth(i.uv.x);
        //float d = i.uv.x / 2;
        //float d = i.uv.x;

        //fixed p = 1 - i.depth;//i.sPos.z / 5;// / i.sPos.w;

        return i.depth;
			
        //return half4(1,0,0,1);
        //return half4(p,p,p,1);
	}

	ENDCG
	
Subshader {
	
 Pass {
	  //ZTest Always Cull Off ZWrite Off
      //ZTest Always ZWrite Off
	  Fog { Mode off }      

      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
}

Fallback off
	
} // shader