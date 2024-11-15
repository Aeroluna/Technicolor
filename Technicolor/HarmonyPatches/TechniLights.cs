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
        private readonly Config _config;

        private TechniLights(LightColorizerManager manager, Config config)
        {
            _manager = manager;
            _config = config;
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(ChromaLightSwitchEventEffect), "BasicCallback")]
        private bool Colorize(ChromaLightSwitchEventEffect __instance, BasicBeatmapEventData beatmapEventData)
        {
            if (!_config.TechnicolorEnabled)
            {
                return true;
            }

            LightColorizer lightColorizer = __instance.Colorizer;
            bool warm = BeatmapEventDataLightsExtensions.GetLightColorTypeFromEventDataValue(beatmapEventData.value) == EnvironmentColorType.Color1;
            if (_config.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
            {
                foreach (ILightWithId light in lightColorizer.Lights)
                {
                    Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time + light.GetHashCode(), _config.TechnicolorLightsStyle);
                    lightColorizer.Colorize(true, color, color, color, color);
                    __instance.Refresh(true, new[] { light }, beatmapEventData);
                }

                return false;
            }

            if (!(TechnicolorController.TechniLightRandom.NextDouble() <
                  _config.TechnicolorLightsFrequency))
            {
                return true;
            }

            {
                Color color = TechnicolorController.GetTechnicolor(warm, beatmapEventData.time, _config.TechnicolorLightsStyle);
                switch (_config.TechnicolorLightsGrouping)
                {
                    case TechnicolorLightsGrouping.ISOLATED_GROUP:
                        lightColorizer.Colorize(true, color, color, color, color);
                        break;

                    default:
                        _manager.GlobalColorize(true, color, color, color, color);
                        break;
                }
            }

            return true;
        }
    }
}
