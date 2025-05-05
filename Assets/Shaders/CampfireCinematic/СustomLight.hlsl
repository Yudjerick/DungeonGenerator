#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

//#if SHADERGRAPH_PREVIEW
//    Direction = half3(0.5, 0.5, 0);
//    Color = 1;
//#else
/*float3 CalcLight(Light l)
{
	
}*/
void CustomLight_float(float3 Position, out float3 Color)
{
    Light light = GetMainLight();
    Color = light.shadowAttenuation;
    for (int i = 0; i < GetAdditionalLightsCount(); i++)
    {
        Color *= GetAdditionalLight(i, Position, 1).shadowAttenuation;
    }
}

#endif