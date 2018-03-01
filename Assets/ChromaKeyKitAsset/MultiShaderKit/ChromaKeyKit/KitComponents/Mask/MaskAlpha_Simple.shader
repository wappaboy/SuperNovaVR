// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChromaKeyKit/Mask/MaskAlpha_Simple" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		float4 position:POSITION;
		half2 texcoord0:TEXCOORD0;
		half2 texcoord1:TEXCOORD1;
	};

	sampler2D _MainTex;
	half4 _MainTex_ST;
	sampler2D _MaskTex;
	half4 _MaskTex_ST;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		o.texcoord1 = TRANSFORM_TEX(input.texcoord, _MaskTex);
		return o;
	}
	half4 frag(VS_OUT input) : SV_Target {
		half4 c = tex2D(_MainTex, input.texcoord0);
		half4 cm = tex2D(_MaskTex, input.texcoord1);
		c.a = cm.a;
		return c;
	}
	ENDCG
	SubShader {
		//Cull Back
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 100

		ZWrite Off
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