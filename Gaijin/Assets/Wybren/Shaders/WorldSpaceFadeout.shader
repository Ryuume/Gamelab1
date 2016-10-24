Shader "Custom/Worldspace Fadeout"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

		_Metallic("Metallic", 2D) = "black" {}
		_Glossiness("Metal Intensity", Range(0,1)) = 0.0

		_NormalMap("Normal", 2D) = "bump" {}

		_DissolveTex("DissolveTex", 2D) = "white" {}
		_DissolvePercentage("DissolvePercentage", Range(0,1)) = 0.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Metallic;
		sampler2D _NormalMap;
		sampler2D _DissolveTex;
		fixed4 _Color;
		float _Scale;
		float _DissolvePercentage;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 worldPos;
			float3 worldNormal; INTERNAL_DATA
		};

		half _Glossiness;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float3 correctWorldNormal = WorldNormalVector(IN, float3(0, 0, 1));
			float2 uv = IN.worldPos.xz;

			if (abs(correctWorldNormal.x) > 0.5) uv = IN.worldPos.yz;
			if (abs(correctWorldNormal.z) > 0.5) uv = IN.worldPos.xy;

			half gradient = tex2D(_DissolveTex, uv).r;
			clip(gradient - _DissolvePercentage);

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color;

			fixed4 m = tex2D(_Metallic, IN.uv_MainTex);
			o.Metallic = m.rgb;

			fixed4 g = tex2D(_Metallic, IN.uv_MainTex);
			o.Smoothness = g.rgb * _Glossiness;

			fixed3 n = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			o.Normal = n;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
