namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Chroma.Lighting;
    using Heck;
    using Technicolor.Settings;
    using UnityEngine;

    // yes i just harmony patched my own mod, you got a problem?
    [HeckPatch(typeof(ChromaLightSwitchEventEffect))]
    [HeckPatch("HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger")]
    [HeckPatch((int)TechniPatchType.LIGHTS)]
    internal static class LightSwitchEventEffectHandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger
    {
        private static bool Prefix(ChromaLightSwitchEventEffect __instance, BeatmapEventData beatmapEventData, BeatmapEventType ____event)
        {
            if (TechnicolorConfig.Instance.TechnicolorEnabled && beatmapEventData.type == ____event &&
                beatmapEventData.value > 0 && beatmapEventData.value <= 7)
            {
                bool warm = !__instance.IsColor0(beatmapEventData.value);
                if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
                {
                    LightColorizer lightColorizer = __instance.LightColorizer;
                    foreach (ILightWithId light in lightColorizer.Lights)
                    {
                        Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time + light.GetHashCode(), TechnicolorConfig.Instance.TechnicolorLightsStyle);
                        lightColorizer.Colorize(false, color, color, color, color);
                        __instance.Refresh(true, new ILightWithId[] { light }, beatmapEventData);
                    }

                    return false;
                }
                else if (TechnicolorController.TechniLightRandom.NextDouble() < TechnicolorConfig.Instance.TechnicolorLightsFrequency)
                {
                    Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle);
                    switch (TechnicolorConfig.Instance.TechnicolorLightsGrouping)
                    {
                        case TechnicolorLightsGrouping.ISOLATED_GROUP:
                            ____event.ColorizeLight(false, color, color, color, color);
                            break;

                        case TechnicolorLightsGrouping.STANDARD:
                        default:
                            LightColorizer.GlobalColorize(false, color, color, color, color);
                            break;
                    }
                }
            }

            return true;
        }
    }
}
