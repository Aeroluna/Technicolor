using Chroma.Colorizer;
using Chroma.Lighting;
using SiraUtil.Affinity;
using Technicolor.Managers;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    // yes i just harmony patched my own mod, you got a problem?
    internal class TechniLights : IAffinity
    {
        private readonly LightColorizerManager _manager;

        private TechniLights(LightColorizerManager manager)
        {
            _manager = manager;
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(ChromaLightSwitchEventEffect), "BasicCallback")]
        private bool Colorize(ChromaLightSwitchEventEffect __instance, BasicBeatmapEventData beatmapEventData)
        {
            if (!TechnicolorConfig.Instance.TechnicolorEnabled)
            {
                return true;
            }

            LightColorizer lightColorizer = __instance.Colorizer;
            bool warm = !ChromaLightSwitchEventEffect.IsColor0(beatmapEventData.value);
            if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
            {
                foreach (ILightWithId light in lightColorizer.Lights)
                {
                    Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time + light.GetHashCode(), TechnicolorConfig.Instance.TechnicolorLightsStyle);
                    lightColorizer.Colorize(false, color, color, color, color);
                    __instance.Refresh(true, new[] { light }, beatmapEventData);
                }

                return false;
            }

            if (!(TechnicolorController.TechniLightRandom.NextDouble() <
                  TechnicolorConfig.Instance.TechnicolorLightsFrequency))
            {
                return true;
            }

            {
                Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle);
                switch (TechnicolorConfig.Instance.TechnicolorLightsGrouping)
                {
                    case TechnicolorLightsGrouping.ISOLATED_GROUP:
                        lightColorizer.Colorize(false, color, color, color, color);
                        break;

                    default:
                        _manager.GlobalColorize(false, color, color, color, color);
                        break;
                }
            }

            return true;
        }
    }
}
