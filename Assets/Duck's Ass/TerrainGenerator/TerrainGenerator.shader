Shader "Unlit/TerrainGenerator"
{
    
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Coof ("Coof", Range(0,1)) = 1 
        _Hight ("Hight", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "./Packages/jp.keijiro.noiseshader/Shader/ClassicNoise2D.hlsl"
            

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float Coefficent;
            float _Hight;
            v2f vert (appdata v)
            {
                
                v2f o;

                float height2 =  (ClassicNoise(v.vertex.zx * 0.5 *Coefficent)) * _Hight;
     
                v.vertex.y = height2 ;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float biom =  ClassicNoise(i.uv * 1.2f *Coefficent) + 1 ;
                float height =  (ClassicNoise(i.uv * 10.2f*Coefficent)/20 + 0.8) * biom;
                float height2 =  (ClassicNoise(i.uv * 1000.2f *Coefficent) + 1)/400 + height;
                fixed4 col = height2;
                

                return col;
            }
            ENDCG
        }
    }
}
