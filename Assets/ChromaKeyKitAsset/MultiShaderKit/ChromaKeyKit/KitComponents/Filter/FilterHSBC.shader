// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChromaKeyKit/Filter/FilterHSBC" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1,1,1,1)
		_Hue("Hue", range(0, 360)) = 0.0
		_Saturation("Saturation", range(-1, 2.0)) = 0.0
		_Brightness("Brightness", range(-1.0, 10.0)) = 0.0
		_Contrast("Contrast", range(0.0, 10.0)) = 1.0
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		float4 position:POSITION;
		half2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	half4 _MainTex_ST;
	
	half4 _BaseColor;
	half _Hue;
	half _Saturation;
	half _Brightness;
	half _Contrast;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return o;
	}
	half3 getBrightness(half3 rgb) {
		rgb *= _Brightness + 1.0;
		return rgb;
	}
	half3 getContrast(half3 rgb) {
		rgb = ((rgb - 0.5f) * _Contrast) + 0.5f;
		return rgb;
	}
	half3 getSaturation(half3 rgb) {
		half3 intensity = dot(rgb, half3(0.299, 0.587, 0.114)); /*half3(0.3, 0.59, 0.91));*/
		rgb = lerp(intensity, rgb, _Saturation + 1.0);
		return rgb;
	}
	half3 getHue(half3 rgb) {
		half angle = radians(_Hue);
		half3 k = half3(0.57735, 0.57735, 0.57735);
		half cosAngle = cos(angle);

		return rgb*cosAngle + cross(k, rgb)*sin(angle) + k*dot(k, rgb)*(1 - cosAngle);
	}

	half4 frag(VS_OUT input) : SV_Target {
		half4 c = tex2D(_MainTex, input.texcoord0)*_BaseColor;
		c.rgb = getHue(c.rgb);
		c.rgb = getContrast(c.rgb);
		c.rgb = getBrightness(c.rgb);
		c.rgb = getSaturation(c.rgb);
		return c;
	}
	ENDCG
	SubShader {
		//Cull back
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

		Lighting Off
		ZWrite Off
		AlphaTest Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			  #pragma vertex vert
			  #pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}