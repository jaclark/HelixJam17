// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "JCG/WavyCube" {
Properties {
	_MainTex ("Texture", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
}
 
SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    Cull Off Lighting Off
    LOD 300
   
 
CGPROGRAM
#pragma surface surf Lambert alpha vertex:vert

sampler2D _MainTex;
fixed4 _Color;
 
struct Input {
    float2 uv_MainTex;
    INTERNAL_DATA
};
 
void vert (inout appdata_full v) {
    float phase = _Time.y * 10;
    float offset = (v.vertex.x) * .005;
    v.vertex.y += sin(phase + offset);
}
 
void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = _Color;
    o.Albedo = c.rgb;
    o.Alpha = 1.0;
}
ENDCG
}
 
FallBack "Reflective/VertexLit"
}