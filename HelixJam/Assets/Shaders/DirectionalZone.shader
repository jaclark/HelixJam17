// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "JCG/Directional Zone"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_LeftEdge("Left Edge", Float) = 0.0
		_RightEdge("Right Edge", Float) = 0.0
		_TopEdge ("Top Edge", Float) = 0.0
		_BottomEdge ("Bottom Edge", Float) = 0.0
	}
	SubShader
	{
		Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_particles
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct appdata
			{
				float4 color : COLOR;
				float4 vertex : POSITION;
				float2 texCoord : TEXCOORD1;
			};

			struct v2f
			{
				float4 color : COLOR;
				float4 vertex : SV_POSITION;
				fixed3 worldPos : TEXCOORD0;
				float2 texCoord : TEXCOORD1;
			};

			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.texCoord = TRANSFORM_TEX(v.texCoord, _MainTex);
				o.color = v.color;
				return o;
			}

			float _LeftEdge = 0;
			float _RightEdge = 0;
			float _TopEdge = 0;
			float _BottomEdge = 0;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.color * tex2D(_MainTex, i.texCoord) * i.color.a;

				if(i.worldPos.x > _LeftEdge && i.worldPos.x < _RightEdge && i.worldPos.y > _BottomEdge && i.worldPos.y < _TopEdge)
				{

				}
				else 
				{
					col.w = 0;
				}

				return col;
			}
			ENDCG
		}
	}
}
