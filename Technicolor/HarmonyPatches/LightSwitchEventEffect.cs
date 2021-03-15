﻿namespace Technicolor.HarmonyPatches
{
    using System.Collections.Generic;
    using Chroma.Colorizer;
    using IPA.Utilities;
    using Technicolor.Settings;
    using UnityEngine;

    [TechniPatch(typeof(LightSwitchEventEffect))]
    [TechniPatch("HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger")]
    [TechniPatch(TechniPatchType.LIGHTS)]
    internal static class LightSwitchEventEffectHandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger
    {
        private static readonly FieldAccessor<LightWithIdManager, List<ILightWithId>[]>.Accessor _lightsWithIdAccessor = FieldAccessor<LightWithIdManager, List<ILightWithId>[]>.GetAccessor("_lights");

        private static bool Prefix(LightSwitchEventEffect __instance, BeatmapEventData beatmapEventData, BeatmapEventType ____event, LightWithIdManager ____lightManager)
        {
            if (TechnicolorConfig.Instance.TechnicolorEnabled && beatmapEventData.type == ____event &&
                beatmapEventData.value > 0 && beatmapEventData.value <= 7)
            {
                if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
                {
                    ____lightManager.SetColorForId(__instance.lightsId, TechnicolorController.GetTechnicolor(beatmapEventData.value > 3, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle));

                    List<ILightWithId>[] lightManagerLights = _lightsWithIdAccessor(ref ____lightManager);
                    List<ILightWithId> lights = lightManagerLights[__instance.lightsId];
                    for (int i = 0; i < lights.Count; i++)
                    {
                        lights[i].ColorWasSet(TechnicolorController.GetTechnicolor(beatmapEventData.value > 3, beatmapEventData.time + lights[i].GetHashCode(), TechnicolorConfig.Instance.TechnicolorLightsStyle));
                    }

                    return false;
                }
                else if (TechnicolorController.TechniLightRandom.NextDouble() < TechnicolorConfig.Instance.TechnicolorLightsFrequency)
                {
                    bool blue = beatmapEventData.value <= 3;
                    switch (TechnicolorConfig.Instance.TechnicolorLightsGrouping)
                    {
                        case TechnicolorLightsGrouping.ISOLATED_GROUP:
                            // ternary operator gore
                            ____event.SetLightingColors(
                                blue ? (Color?)TechnicolorController.GetTechnicolor(false, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle) : null,
                                blue ? null : (Color?)TechnicolorController.GetTechnicolor(true, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle));
                            break;

                        case TechnicolorLightsGrouping.STANDARD:
                        default:
                            Color? t = TechnicolorController.GetTechnicolor(!blue, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle);
                            LightColorizer.SetAllLightingColors(blue ? null : t, blue ? t : null);
                            break;
                    }
                }
            }

            return true;
        }
    }
}
