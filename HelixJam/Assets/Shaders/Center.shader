Shader "JCG/HelixCenterLine"
{
	Properties
	{
		_LineColor ("Line Color", Color) = (0,0,0,0)
		_LineWidth ("Line Width", Float) = 0.0
		_ColorA ("Color A", Color) = (0,0,0,0)
		_ColorB ("Color B", Color) = (0,0,0,0)
		_XPosA ("X Position A", Float) = 0.0
		_XPosB ("X Position B", Float) = 0.0
		_GlowWidth("Glow Width", Float) = 0.0
		_MaxProximity("Max Proximity", Float) = 0.0
		_Alpha ("Alpha", Float) = 0.0
	}
	SubShader
	{
		Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"



			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldSpacePosition : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.worldSpacePosition = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 _LineColor;
			float _LineWidth;
			fixed4 _ColorA;
			fixed4 _ColorB;
			float _XPosA;
			float _XPosB;
			float _GlowWidth;
			float _Proximity;
			float _MaxProximity;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 0;

				float distanceFromZero = abs(i.worldSpacePosition.x);

				if(distanceFromZero < _LineWidth/2)
				{
					col = _LineColor;
				}

				if(distanceFromZero > _LineWidth/2 && distanceFromZero < _GlowWidth/2)
				{
					col = 1;
					float proxScaler = 1;

					float proxA = clamp(abs(_XPosA), 0, _MaxProximity);
					float proxScalerA = (proxA) / _MaxProximity;

					float proxB = clamp(abs(_XPosB), 0, _MaxProximity);
					float proxScalerB = (proxB) / _MaxProximity;

					bool dontShow = false;
					if(i.worldSpacePosition.x > 0)
					{ 
						if(_XPosA > 0 && _XPosB > 0)
						{
							col = lerp(_ColorA, _ColorB, .5);
							proxScaler = max(proxScalerA, proxScalerB);
						}
						else if(_XPosA > 0)
						{
							col = _ColorA;
							proxScaler = proxScalerA;
						}
						else if(_XPosB > 0)
						{
							col = _ColorB;
							proxScaler = proxScalerB;
						}
						else
						{
							dontShow = true;
						}
					}

					if(i.worldSpacePosition.x < 0)
					{
						if(_XPosA <= 0 && _XPosB <= 0)
						{
							col = lerp(_ColorA, _ColorB, .5);
							proxScaler = max(proxScalerA, proxScalerB);
						}
						else if(_XPosA <= 0)
						{
							col = _ColorA;
							proxScaler = proxScalerA;
						}
						else if(_XPosB <= 0)
						{
							col = _ColorB;
							proxScaler = proxScalerB;
						}
						else
						{
							dontShow = true;
						}
					}

					if(!dontShow)
					{
						col.w = (_GlowWidth/2 - abs(i.worldSpacePosition.x)) / _GlowWidth/2;
						col.w *= proxScaler;
						col.w *= _Alpha;
					}
					else
					{
						col.w = 0;
					}
				}

				return col;
			}
			ENDCG
		}
	}
}
