using SiraUtil.Affinity;
using Technicolor.Managers;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    // could technically just be a static harmony patch but i just want to handle it with every other patch
    internal class TechniParticles : IAffinity
    {
        private readonly Config _config;

        private TechniParticles(Config config)
        {
            _config = config;
        }

        [AffinityPrefix]
        [AffinityPatch(
            typeof(ParticleSystemEventEffect),
            nameof(ParticleSystemEventEffect.HandleBeatmapEvent))]
        private bool Colorize(ParticleSystemEventEffect __instance, BasicBeatmapEventData basicBeatmapEventData)
        {
            if (!_config.TechnicolorEnabled)
            {
                return true;
            }

            if (_config.TechnicolorLightsGrouping != TechnicolorLightsGrouping.ISOLATED ||
                !(TechnicolorController.TechniLightRandom.NextDouble() <
                  _config.TechnicolorLightsFrequency))
            {
                return true;
            }

            ParticleSystem.MainModule mainmodule = __instance._mainModule;
            ParticleSystem.Particle[] particles = __instance._particles;
            ParticleSystem particleSystem = __instance._particleSystem;
            mainmodule.startColor = TechnicolorController.GetTechnicolor(
                basicBeatmapEventData.value > 3,
                basicBeatmapEventData.time,
                _config.TechnicolorLightsStyle);
            particleSystem.GetParticles(particles, particles.Length);
            for (int i = 0; i < particleSystem.particleCount; i++)
            {
                particles[i].startColor = TechnicolorController.GetTechnicolor(
                    basicBeatmapEventData.value > 3,
                    basicBeatmapEventData.time + particles[i].randomSeed,
                    _config.TechnicolorLightsStyle);
            }

            particleSystem.SetParticles(particles, particleSystem.particleCount);

            return false;
        }
    }
}
