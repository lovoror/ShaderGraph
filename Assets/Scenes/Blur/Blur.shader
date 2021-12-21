Shader "PostEffect/Blur" {
    Properties {
        _MainTex ("Base Texture", 2D) = "white" {}
    }
    SubShader {
        Cull Off ZWrite Off ZTest Always
        Tags { "RenderPipeline" = "UniversalPipeline"  }
            HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float2 _FocusScreenPosition;
            float _FocusPower;
            
            CBUFFER_END
            ENDHLSL
            
        pass{
            Tags{"LightMode"="UniversalForward"}//这个Pass最终会输出到颜色缓冲里
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            struct appdata
            {
                float4 vertex:POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f
            {
                float2 uv:TEXCOORD0;
                 float4 vertex: SV_POSITION;
            };

           
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

             v2f Vert(appdata IN)
            {
                v2f OUT;
                //在CG里面，我们这样转换空间坐标 o.vertex = UnityObjectToClipPos(v.vertex);
                 VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.vertex.xyz);
                 OUT.vertex = positionInputs.positionCS;
                 OUT.uv = TRANSFORM_TEX(IN.uv,_MainTex);
                // o.vertex = UnityObjectToClipPos(v.vertex);
                return OUT;
            }
            
            float4 Frag(v2f i): SV_Target
            {
                float2 uv = i.uv;

                half2 uv1 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * - 2.0;
                half2 uv2 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * - 1.0;
                half2 uv3 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 0.0;
                half2 uv4 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 1.0;
                half2 uv5 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 2.0;
                half4 s = 0;

                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv1) * 0.0545;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv2) * 0.2442;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv3) * 0.4026;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv4) * 0.2442;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv5) * 0.0545;

                return s;
            }
            
            ENDHLSL
        }
        
        pass{
//            Tags{"LightMode"="UniversalForward"}//这个Pass最终会输出到颜色缓冲里
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            struct appdata
            {
                float4 vertex:POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f
            {
                float2 uv:TEXCOORD0;
                 float4 vertex: SV_POSITION;
            };

           
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

             v2f Vert(appdata IN)
            {
                v2f OUT;
                //在CG里面，我们这样转换空间坐标 o.vertex = UnityObjectToClipPos(v.vertex);
                 VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.vertex.xyz);
                 OUT.vertex = positionInputs.positionCS;
                 OUT.uv = TRANSFORM_TEX(IN.uv,_MainTex);
                // o.vertex = UnityObjectToClipPos(v.vertex);
                return OUT;
            }
            
            float4 Frag(v2f i): SV_Target
            {
                float2 uv = i.uv;

                half2 uv1 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * - 2.0;
                half2 uv2 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * - 1.0;
                half2 uv3 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 0.0;
                half2 uv4 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 1.0;
                half2 uv5 = uv + half2(_FocusPower / _ScreenParams.x, _FocusPower / _ScreenParams.y) * half2(1, 0) * 2.0;
                half4 s = 0;

                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv1) * 0.0545;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv2) * 0.2442;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv3) * 0.4026;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv4) * 0.2442;
                s += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv5) * 0.0545;

                return s;
            }
            
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}