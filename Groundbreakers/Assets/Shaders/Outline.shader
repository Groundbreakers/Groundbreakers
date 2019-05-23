// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline"
{
       Properties
       {
           _Color ("Colour", Color) = (1,1,1,1)
           _MainTex ("Texture", 2D) = "white" {}
           _TurnOn ("TurnOn", float) = 0
           [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
       }

       SubShader
       {
           Tags{"Queue" = "Transparent"}
           
           Cull Off
           ZTest Off
           ZWrite On
           Blend One OneMinusSrcAlpha

           Pass
           {
               CGPROGRAM

               #pragma vertex vertexFunc
               #pragma fragment fragmentFunc
               #pragma multi_compile _ PIXELSNAP_ON
               #include "UnityCG.cginc"

               sampler2D _MainTex;
               float _TurnOn;

               struct v2f {
                  float4 pos : SV_POSITION;
                  half2 uv : TEXCOORD0;
               };

               v2f vertexFunc(appdata_base v) {
                  v2f o;
                  o.pos = UnityObjectToClipPos(v.vertex);
                  o.uv = v.texcoord;

                  return o;
               }

               fixed4 _Color;
               float4 _MainTex_TexelSize;

               fixed4 fragmentFunc(v2f i) : COLOR {
                  half4 c = tex2D(_MainTex, i.uv);

                  c.rgb *= c.a;

                  if (_TurnOn != 0)
                  {
                     half4 outlineC = _Color;
                     outlineC.a *= ceil(c.a);
                     outlineC.rgb = outlineC.a;

                     fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
                     fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
                     fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
                     fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

                     return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
                  }
                  else {
                     return c;
                  }

               }


               ENDCG
           }
       }
}
