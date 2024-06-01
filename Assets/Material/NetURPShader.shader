Shader "Custom/TransparentNetURPShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize ("Grid Size", Float) = 10.0
        _LineWidth ("Line Width", Range(0.0, 1.0)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _GridSize;
            float _LineWidth;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float2 grid = frac(input.uv * _GridSize);
                float2 lines = step(grid, _LineWidth) + step(1.0 - grid, _LineWidth);
                float Line = lines.x * lines.y;
                float alpha = 1.0 - Line;
                return half4(1, 1, 1, alpha); // White net lines with transparency
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
