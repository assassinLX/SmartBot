// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/test"
{
	Properties
	{
		_MainTex("Base(RGB) Gloss(A)", 2D) = "white" {}
		_Specular("Specular", Range(0, 10)) = 1
		_Shininess("Shininess", Range(0.01, 1)) = 0.5
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed _Specular;
			fixed _Shininess;

			struct a2v
			{
				fixed4 vertex : POSITION;
				fixed3 normal : NORMAL;
				fixed4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed3 spec : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				fixed3 normal = mul((fixed3x3)unity_ObjectToWorld, SCALED_NORMAL);
				fixed3 viewDir = normalize(WorldSpaceViewDir(v.vertex));
				fixed3 lightDir = normalize(WorldSpaceLightDir(v.vertex));
				fixed nh = saturate(dot(normal, normalize(viewDir + lightDir)));

				o.spec = _LightColor0.rgb * pow(nh, _Shininess * 128) * _Specular;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.uv.xy);
				fixed4 o = c;

				o.rgb += i.spec * c.a;

				return o;
			}

			ENDCG
		}
	}

	FallBack "Mobile/Diffuse"
}
