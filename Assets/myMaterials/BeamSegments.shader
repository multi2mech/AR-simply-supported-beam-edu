Shader "Custom/DistanceColorShader"
{
    Properties
    {
        _ReferencePoint ("Reference Point", Vector) = (0, 0, 0, 1)  // Reference point for distance calculations
        _NumThresholds ("Number of Thresholds", int) = 4  // Number of thresholds (passed from C#)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _ReferencePoint;   // Position reference
            int _NumThresholds;       // Number of thresholds
            float _Thresholds[10];    // Supports up to 10 distance thresholds
            fixed4 _Colors[10];       // Supports up to 10 colors

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Compute distance from reference point
                float dist = distance(i.worldPos, _ReferencePoint.xyz);

                // Find the correct color index using floor()
                int index = 0;
                for (int j = 0; j < _NumThresholds; j++)
                {
                    if (dist >= _Thresholds[j])
                        index = j + 1;
                }

                // Clamp index to prevent out-of-bounds errors
                index = clamp(index, 0, _NumThresholds - 1);

                return _Colors[index]; // Return assigned color
            }
            ENDCG
        }
    }
}