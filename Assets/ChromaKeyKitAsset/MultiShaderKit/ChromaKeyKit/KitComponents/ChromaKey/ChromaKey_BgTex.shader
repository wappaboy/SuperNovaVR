// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChromaKeyKit/ChromaKey/ChromaKey_BgTex" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_BgTex("BgTex", 2D) = "white" {}
		_KeyColor("KeyColor", Color) = (1,1,1,1)
		_DChroma("D Chroma", range(0.0, 1.0)) = 0.5
		_DChromaT("D Chroma Tolerance", range(0.0, 1.0)) = 0.05

		_Chroma("Chroma (Main -> Bg)", range(0.0, 1.0)) = 0.5
		_Luma("Luma (Main -> Bg)", range(0.0, 1.0)) = 0.5
		_Saturation("Saturation (0 -> Chroma)", range(0.0, 1.0)) = 1.0
		_Alpha("Alpha (Chroma -> Bg)", range(0.0, 1.0)) = 1.0
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		float4 position:POSITION;
		half2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	half4 _MainTex_ST;
	
	sampler2D _BgTex;
	half4 _BgTex_ST;

	half4 _KeyColor;
	half _DChroma;
	half _DChromaT;
	
	half _Chroma;
	half _Luma;
	half _Saturation;
	half _Alpha;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return o;
	}

	half3 RGB_To_YCbCr(half3 RGB) {
		half Y = 0.299 * RGB.r + 0.587 * RGB.g + 0.114 * RGB.b;
		half Cb = 0.564 * (RGB.b - Y);
		half Cr = 0.713 * (RGB.r - Y);
		return half3(Cb, Cr, Y);
	}

	half3 YCbCr_To_RGB(half3 YCbCr) {
		half R = YCbCr.z + 1.402 * YCbCr.y;
		half G = YCbCr.z - 0.334 * YCbCr.x - 0.714 * YCbCr.y;
		half B = YCbCr.z + 1.772 * YCbCr.x;
		return half3(R, G, B);
	}
	half2 lerp3_2(half2 A, half2 B, half2 C, half v) {
		if (v < 0.5) {
			return lerp(A, B, 2*v);
		}
		else {
			return lerp(B, C, 2*(v-0.5));
		}
	}
	half lerp3_1(half A, half B, half C, half v) {
		if (v < 0.5) {
			return lerp(A, B, 2 * v);
		}
		else {
			return lerp(B, C, 2 * (v - 0.5));
		}
	}
	half4 frag(VS_OUT input) : SV_Target {
		half4 color = tex2D(_MainTex, input.texcoord0);
		half4 color_bg = tex2D(_BgTex, input.texcoord0);

		half3 src_YCbCr = RGB_To_YCbCr(color.rgb);
		half3 target_YCbCr = RGB_To_YCbCr(color_bg.rgb);
		half3 key_YCbCr = RGB_To_YCbCr(_KeyColor);

		half dChroma = distance(src_YCbCr.xy, key_YCbCr.xy);
		if (dChroma < _DChroma) {
			half a = 0;
			color.rgba = color_bg.rgba;
			if (dChroma > _DChroma - _DChromaT) {
				a = (dChroma - _DChroma + _DChromaT) / _DChromaT;
				half2 ca = lerp(src_YCbCr.xy, target_YCbCr.xy, 1-a);
				half2 c = lerp3_2(src_YCbCr.xy, ca, target_YCbCr.xy, _Chroma);

				half sa = length(ca);
				half s = lerp(0, sa, _Saturation);
				c *= s / sa;

				half la = lerp(src_YCbCr.z, target_YCbCr.z, 1-a);
				half l = lerp3_1(src_YCbCr.z, la, target_YCbCr.z, _Luma);

				color.rgb = YCbCr_To_RGB(float3(c.x, c.y, l));
			}
			color.a = lerp(a, color.a, _Alpha);
		}
		
		return color;
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