// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChromaKeyKit/Blur/Blur_111-111-111" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_BlurOffsetX ("_BlurOffsetX", float) = 3
		_BlurOffsetY("_BlurOffsetY", float) = 3
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		float4 position:POSITION;
		half2 texcoord0:TEXCOORD9;
		half2 texcoords[8]:TEXCOORD0;
	};

	sampler2D _MainTex;
	half4 _MainTex_ST;
	half4 _MainTex_TexelSize;
	half _BlurOffsetX;
	half _BlurOffsetY;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		half2 offsetX = _MainTex_TexelSize * half2(0.5*_BlurOffsetX, 0);
		half2 offsetY = _MainTex_TexelSize * half2(0, 0.5*_BlurOffsetY);
		half2 texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		
		o.texcoord0 = texcoord0;
		o.texcoords[0] = texcoord0 + offsetX;
		o.texcoords[1] = texcoord0 - offsetX;
		o.texcoords[2] = texcoord0 + offsetY;
		o.texcoords[3] = texcoord0 - offsetY;

		o.texcoords[4] = texcoord0 + offsetX + offsetY;
		o.texcoords[5] = texcoord0 + offsetX - offsetY;
		o.texcoords[6] = texcoord0 - offsetX + offsetY;
		o.texcoords[7] = texcoord0 - offsetX - offsetY;
		return o;
	}
	half4 frag(VS_OUT input) : SV_Target {
		half4 c = tex2D(_MainTex, input.texcoord0);
		c += tex2D(_MainTex, input.texcoords[0]);
		c += tex2D(_MainTex, input.texcoords[1]);
		c += tex2D(_MainTex, input.texcoords[2]);
		c += tex2D(_MainTex, input.texcoords[3]);
		c += tex2D(_MainTex, input.texcoords[4]);
		c += tex2D(_MainTex, input.texcoords[5]);
		c += tex2D(_MainTex, input.texcoords[6]);
		c += tex2D(_MainTex, input.texcoords[7]);
		return 0.111 * c;
	}
	ENDCG
	SubShader {
		//Cull Back
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

		Lighting Off
		ZWrite Off
		AlphaTest Off
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			  #pragma vertex vert
			  #pragma fragment frag
			ENDCG
		}
	}
	Fallback off
}