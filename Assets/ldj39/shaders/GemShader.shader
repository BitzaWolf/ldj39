/**
 * Based on the Basic shader, this one adds a small tweak to make gems appear extra shiny.
 * A surface shader that either draws greyscale, or fully textured colors based on
 * the redness of light hitting it. This basically just enables lights to cause things
 * to become colored instead of traditional lighting.
 */

Shader "Bitzawolf/Gem Surface"
{
    Properties
	{
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
    }

    SubShader
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Custom

		half4 LightingCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half NdotL = dot (s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half NdotV = dot(s.Normal, viewDir);

			half4 c;
			c.rgb = s.Albedo * (diff * atten) * _LightColor0.b;

			// THIS IS GEM SHADER!
			half difference = abs(NdotL - NdotV);
			if (difference < 0.05)
			{
				half4 shine = half4(_LightColor0.b,  _LightColor0.b, _LightColor0.b, 1) * .9;
				float shineRamp = (1 - difference - 0.95) / 0.05;
				c = lerp(c, shine, shineRamp);
			}
			// END EXCITEMENT

			c.a = s.Alpha;

			float average = (c.r + c.g + c.b) / 3;
			half4 greyscale = half4(average, average, average, c.a);

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
		}
		ENDCG
    }

    Fallback "Diffuse"
}