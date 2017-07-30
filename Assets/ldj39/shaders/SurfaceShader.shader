/**
 * A surface shader that either draws greyscale, or fully textured colors based on
 * the redness of light hitting it. This basically just enables lights to cause things
 * to become colored instead of traditional lighting.
 */

Shader "Bitzawolf/Basic Surface"
{
    Properties
	{
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
    }

    SubShader
	{
		Tags { "RenderType" = "Transparent" }

		CGPROGRAM
		#pragma surface surf Custom

		half4 LightingCustom (SurfaceOutput s, half3 lightDir, half atten)
		{
			half lightness = (_LightColor0.b + _LightColor0.r) / 2;

			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;

			half4 c;
			c.rgb = s.Albedo * (diff * atten) * lightness;

			float average = (c.r + c.g + c.b) / 3;
			half4 greyscale = half4(average, average, average, 1);

			return lerp(greyscale, c, _LightColor0.r);
		}

		struct Input
		{
			float2 uv_MainTex;
		};
    
		sampler2D _MainTex;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
			o.Alpha = _Color.a;
		}
		ENDCG
    }

    Fallback "Diffuse"
}