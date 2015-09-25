Shader "Toon/ToonyPolymer" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_Shininess ("Shininess", Range (0.1, 1)) = 0.7
		_Intensity ("Intensity", Range (0.1, 5.0)) = 1.0
 
	}
 
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp
 
sampler2D _Ramp;
 
// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, fixed3 halfDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
    	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (d, s.Specular*128) * s.Gloss;
	fixed diff = nh;
 
	half4 c;
	float modifier = d;
	
	if (modifier > 0.2)
		modifier += (modifier-0.1)/4.0;
 
	c.rgb = (s.Albedo * _LightColor0.rgb * ramp + _LightColor0.rgb * spec) * (atten*2*modifier);
 
	c.a = 0;
 
	return c;
}
 
sampler2D _MainTex;
float4 _Color;
half _Shininess;
float _Intensity;
 
struct Input {
	float2 uv_MainTex : TEXCOORD0;
};
 
void surf (Input IN, inout SurfaceOutput o) {
        fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
 
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * _Intensity;
	
	o.Albedo = c.rgb;
	o.Alpha = tex.a;
	o.Gloss = _Shininess;
	o.Specular = _Shininess;
 
}
ENDCG
 
	} 
 
	Fallback "Diffuse"
}