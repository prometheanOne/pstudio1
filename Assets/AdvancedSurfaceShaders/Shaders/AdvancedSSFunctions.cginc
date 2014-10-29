#ifndef ADVANCEDSSFUNCTIONS_CG_INCLUDED
#define ADVANCEDSSFUNCTIONS_CG_INCLUDED

#ifdef ADVANCEDSS_LIGHTINGRAMP
inline fixed4 LightingBlinnPhongRamp (SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{
    fixed3 h = normalize (lightDir + viewDir);
      	
    half NdotL = dot (s.Normal, lightDir);
    half3 diff = tex2D (_LightingRamp, float2(NdotL * 0.5 + 0.5,0));
      	
    float nh = max (0, dot (s.Normal, h));
    float spec = pow (nh, s.Specular*128.0) * s.Gloss;
      	
    fixed4 c;
    c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
    c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
    return c;
}

inline fixed4 LightingBlinnPhongRamp_PrePass (SurfaceOutput s, half4 light)
{
    half d = Luminance(light.rgb);
    half3 diff = tex2D (_LightingRamp, float2(d,d)).rgb;

	fixed spec = light.a * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * light.rgb * diff + light.rgb * _SpecColor.rgb * spec);
	c.a = s.Alpha + spec * _SpecColor.a;
	return c;
}

inline fixed4 LightingMobileBlinnPhongRamp (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
    half NdotL = dot (s.Normal, lightDir);
    half3 diff = tex2D (_LightingRamp, float2(NdotL * 0.5 + 0.5,0));

	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten*2);
	c.a = 0.0;
	return c;
}

inline fixed4 LightingLambertRamp (SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{
    fixed3 h = normalize (lightDir + viewDir);
      	
    half NdotL = max (0, dot (s.Normal, lightDir));
    half3 diff = tex2D (_LightingRamp, float2(NdotL * 0.5 + 0.5,0));
	
	fixed4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingLambertRamp_PrePass (SurfaceOutput s, half4 light)
{
    half d = Luminance(light.rgb);
    half3 diff = tex2D (_LightingRamp, float2(d,d)).rgb;

	fixed4 c;
	c.rgb = s.Albedo * light.rgb * diff;
	c.a = s.Alpha;
	return c;
}
#endif

#ifdef ADVANCEDSS_TESSELLATION
struct appdata
{
    float4 vertex : POSITION;
#ifdef ADVANCEDSS_BUMP
	float4 tangent : TANGENT;
#endif
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

#ifdef ADVANCEDSS_TESSELLATIONPHONG
void DispNone (inout appdata v) { }
#else
void Disp (inout appdata v)
{
    float d = tex2Dlod(_SpecMap, float4(v.texcoord.xy,0,0)).a * _Displacement;
    v.vertex.xyz += v.normal * d;
}
#endif

#ifdef ADVANCEDSS_TESSELLATIONMAPFIXED
float4 TessFixed ()
{
    return _Tess;
}
#endif

#ifdef ADVANCEDSS_TESSELLATIONMAPDISTANCE
float4 TessDistance (appdata v0, appdata v1, appdata v2)
{
    return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, _TessDistanceMin, _TessDistanceMax, _Tess);
}
#endif

#if defined(ADVANCEDSS_TESSELLATIONMAPEDGELENGTH) || defined(ADVANCEDSS_TESSELLATIONPHONG)
float4 TessEdge (appdata v0, appdata v1, appdata v2)
{
    return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
}
#endif
#endif

#ifdef ADVANCEDSS_MOBILE
inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten*2);
	c.a = 0.0;
	return c;
}
#endif

#ifdef ADVANCEDSS_POM
inline float2 ParallaxOcclusionOffset (
	float3 pViewDir, float parallaxHeight, float3 normal,
	float2 uv, sampler2D heightMap, float maxSamples )
{
	//viewDirNorm = normalize( pViewDir );
	//viewDirNorm.z += 0.42;
	
	float2 vParallaxDirection = normalize( pViewDir.xy );
    
    float fLength         = length( pViewDir );
    float fParallaxLength = sqrt( fLength * fLength - pViewDir.z * pViewDir.z ) / pViewDir.z;

    float2 vParallaxOffsetTS = vParallaxDirection * fParallaxLength;
    
    vParallaxOffsetTS *= parallaxHeight;
    
	//#if !defined(SHADER_API_OPENGL)
    float nMinSamples = 6;
    //#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
    #if defined(SHADER_API_D3D11_9X)
    float nMaxSamples = 30;
    #else
    float nMaxSamples = min(maxSamples, 50);
    #endif
    int nNumSamples = (int)(lerp( nMinSamples, nMaxSamples, 1-dot( pViewDir, normal ) ));
    float fStepSize = 1.0 / (float)nNumSamples;   
    int    nStepIndex = 0;
    //#else
    //float fStepSize = 0.03125;
    //#endif
    float fCurrHeight = 0.0;
    float fPrevHeight = 1.0;
    float2 vTexOffsetPerStep = fStepSize * vParallaxOffsetTS;
    float2 vTexCurrentOffset = uv;
    float  fCurrentBound     = 1.0;
    float  fParallaxAmount   = 0.0;

    float2 pt1 = 0;
    float2 pt2 = 0;
    
    //#if !defined(SHADER_API_OPENGL)
    //while ( nStepIndex < nNumSamples )
    #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
    for (nStepIndex = 0; nStepIndex < 30; nStepIndex++)
    #else
    for (nStepIndex = 0; nStepIndex < nNumSamples; nStepIndex++)
    #endif
    {
        vTexCurrentOffset -= vTexOffsetPerStep;
        
        #if defined(SHADER_API_D3D11_9X)
        fCurrHeight = tex2D( heightMap, vTexCurrentOffset).w;
        #else
        fCurrHeight = tex2Dlod( heightMap, float4(vTexCurrentOffset,0,0)).w;
        #endif

        fCurrentBound -= fStepSize;

        if ( fCurrHeight > fCurrentBound ) 
        {   
           pt1 = float2( fCurrentBound, fCurrHeight );
           pt2 = float2( fCurrentBound + fStepSize, fPrevHeight );

           #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
           break;
           #else
           nStepIndex = nNumSamples + 1;   //Exit loop
           #endif
           fPrevHeight = fCurrHeight;
        }
        else
        {
           //nStepIndex++;
           fPrevHeight = fCurrHeight;
        }
    }
    //#else
    //float done = 0;
    //float done2 = 0;
    //for(int i=0; i < 32; i++)
    //{
    //    vTexCurrentOffset -= vTexOffsetPerStep;
    //    
    //    //fCurrHeight = tex2Dlod( heightMap, float4(vTexCurrentOffset,0,0)).w;
    //    fCurrHeight = tex2D( heightMap, vTexCurrentOffset).w;
    //
    //    fCurrentBound -= fStepSize;
    //    
    //    done = step( fCurrentBound, fCurrHeight - done2 );
    //    done2 += done * 100;
    //
    //    pt1 += float2( fCurrentBound * done, fCurrHeight * done );
    //    pt2 += float2( (fCurrentBound + fStepSize) * done, fPrevHeight * done );
    //
    //    fPrevHeight = fCurrHeight;
    //}
    //#endif
    
    float fDelta2 = pt2.x - pt2.y;
    float fDelta1 = pt1.x - pt1.y;
      
    float fDenominator = fDelta2 - fDelta1;
    
    if ( fDenominator == 0.0f )
    {
        fParallaxAmount = 0.0f;
    }
    else
    {
        fParallaxAmount = (pt1.x * fDelta2 - pt2.x * fDelta1 ) / fDenominator;
    }
    
    return vParallaxOffsetTS * (1 - fParallaxAmount );
}
#endif

#define DEPTH_BIAS
#define BORDER_CLAMP

#ifdef ADVANCEDSS_RELIEF
inline float2 RelaxedConeStep (
	float3 pViewDir, float parallaxHeight, float2 uv, sampler2D coneMap, float4 clipTiling )
{
	float3 p,v;	
	
	p = float3(uv,0);
	v = normalize(pViewDir*-1);
	
	v.z = abs(v.z);
	
#ifdef DEPTH_BIAS
	float db = 1.0-v.z; db*=db; db*=db; db=1.0-db*db;
	v.xy *= db;
#endif

	v.xy *= parallaxHeight;
	
	const int cone_steps=20;
	const int binary_steps=10;
	
	float3 p0 = p;

	v /= v.z;
	
	float dist = length(v.xy);
	
	for( int i = 0; i < cone_steps; i++ )
	{
		float4 tex = tex2D(coneMap, p.xy);
		
		float height = saturate(tex.w - p.z);
		
		float cone_ratio = tex.z;
		
		p += v * (cone_ratio * height / (dist + cone_ratio));
	}

	v *= p.z*0.5;
	p = p0 + v;

	for( int j = 0; j < binary_steps; j++ )
	{
		float4 tex = tex2D(coneMap, p.xy);
		v *= 0.5;
		if (p.z<tex.w)
			p+=v;
		else
			p-=v;
	}
	
#ifdef BORDER_CLAMP
    clip ( p.xy + clipTiling.zw + 0.01 );
    clip ( step( p.x - 0.01, clipTiling.x ) - 0.9 );
    clip ( step( p.y - 0.01, clipTiling.y ) - 0.9 );
#endif

    return p.xy;
}
#endif

#endif
