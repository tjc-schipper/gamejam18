Shader "Hidden/DiscoColors"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Rainbow ("Texture", 2D) = "white" {}
		_Speed ("Speed", Float) = 1.0
		_Strength ("Strength", Float) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _Rainbow;
			half _Speed;
			half _Strength;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 rainbow = tex2D(_Rainbow, float2(sin(_Time.x * _Speed) / 2.0 + 0.5, 0)) * _Strength;
				return saturate(col + rainbow);
			}
			ENDCG
		}
	}
}
