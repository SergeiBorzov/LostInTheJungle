Shader "Custom/Diffuse" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader{
		Tags {"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}
		LOD 200
		Cull Off

		CGPROGRAM
		#pragma surface surf Standard addshadow

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			clip(0.5);
			//o.Alpha = c.a;
		}
		ENDCG
		}
		FallBack "Diffuse"
}