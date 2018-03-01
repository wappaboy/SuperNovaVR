// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChromaKeyKit/ChromaKey/ChromaKey_Alpha_Simple" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_KeyColor("KeyColor", Color) = (1,1,1,1)
		_DChroma("D Chroma", range(0.0, 1.0)) = 0.5
		_DChromaT("D Chroma Tolerance", range(0.0, 1.0)) = 0.05
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		float4 position:POSITION;
		half2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	half4 _MainTex_ST;
	
	half4 _KeyColor;
	half _DChroma;
	half _DChromaT;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return o;
	}

	half3 RGB_To_YCbCr(half3 rgb) {
		half Y = 0.299 * rgb.r + 0.587 * rgb.g + 0.114 * rgb.b;
		half Cb = 0.564 * (rgb.b - Y);
		half Cr = 0.713 * (rgb.r - Y);
		return half3(Cb, Cr, Y);
	}

	half4 frag(VS_OUT input) : SV_Target {
		half4 c = tex2D(_MainTex, input.texcoord0);

		half3 src_YCbCr = RGB_To_YCbCr(c.rgb);
		half3 key_YCbCr = RGB_To_YCbCr(_KeyColor);

		half dChroma = distance(src_YCbCr.xy, key_YCbCr.xy);

		if (dChroma < _DChroma) {
			half a = 0;
			if (dChroma > _DChroma - _DChromaT) {
				a = (dChroma -_DChroma + _DChromaT) / _DChromaT;
			}
			c.a = a;
		}
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