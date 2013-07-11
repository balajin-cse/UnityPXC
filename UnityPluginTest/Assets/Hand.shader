Shader "Custom/Hand" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Specular ("Specular", 2D) = "specular" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Specular;
		float _SpecParam;
		float _GlossParam;
		float4 _SkinTone;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Specular = tex2D (_Specular, IN.uv_MainTex).x;
			o.Gloss = 1;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
