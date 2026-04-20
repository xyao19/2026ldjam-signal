Shader "UI/RadarBlip"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlipColor ("Blip Color", Color) = (0,1,0,1)
        _BlinkSpeed ("Blink Speed (Hz)", Float) = 2.0
        _Intensity ("Base Intensity", Range(0,1)) = 1.0
        
        // UI Shader 必需属性
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // Stencil 裁剪支持 (UI 必需)
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [UnityZ_ZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _BlipColor;
            float _BlinkSpeed;
            float _Intensity;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 获取贴图颜色（通常是一个圆形的 Alpha 遮罩）
                fixed4 col = tex2D(_MainTex, i.texcoord);
                
                // --- 闪烁核心逻辑 ---
                // 使用正弦波 (_Time.y 是时间，秒)
                // 角度 = 时间 * 速度 * 2PI
                float angle = _Time.y * _BlinkSpeed * 6.283185;
                
                // 将 sin 范围 [-1, 1] 映射到 [0, 1] 
                float blinkFactor = sin(angle) * 0.5 + 0.5;
                
                // 应用亮度倍数
                blinkFactor *= _Intensity;

                // 最终颜色：自定义颜色 * 贴图 Alpha * 闪烁因子
                fixed4 finalCol = _BlipColor;
                finalCol.a = col.a * blinkFactor;
                
                // 简单的防锯齿裁剪
                clip(finalCol.a - 0.01);

                return finalCol;
            }
            ENDCG
        }
    }
}