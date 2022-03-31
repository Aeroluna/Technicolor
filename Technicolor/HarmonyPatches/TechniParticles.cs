using IPA.Utilities;
using SiraUtil.Affinity;
using Technicolor.Managers;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    // could technically just be a static harmony patch but i just want to handle it with every other patch
    internal class TechniParticles : IAffinity
    {
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem.MainModule>.Accessor _mainModuleAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem.MainModule>.GetAccessor("_mainModule");
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem.Particle[]>.Accessor _particlesAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem.Particle[]>.GetAccessor("_particles");
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem>.Accessor _particleSystemAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem>.GetAccessor("_particleSystem");

        [AffinityPrefix]
        [AffinityPatch(
            typeof(ParticleSystemEventEffect),
            nameof(ParticleSystemEventEffect.HandleBeatmapEvent))]
        private bool Colorize(ParticleSystemEventEffect __instance, BasicBeatmapEventData basicBeatmapEventData)
        {
            if (!TechnicolorConfig.Instance.TechnicolorEnabled)
            {
                return true;
            }

            if (TechnicolorConfig.Instance.TechnicolorLightsGrouping != TechnicolorLightsGrouping.ISOLATED ||
                !(TechnicolorController.TechniLightRandom.NextDouble() <
                  TechnicolorConfig.Instance.TechnicolorLightsFrequency))
            {
                return true;
            }

            ParticleSystem.MainModule mainmodule = _mainModuleAccessor(ref __instance);
            ParticleSystem.Particle[] particles = _particlesAccessor(ref __instance);
            ParticleSystem particleSystem = _particleSystemAccessor(ref __instance);
            mainmodule.startColor = TechnicolorController.GetTechnicolor(
                basicBeatmapEventData.value > 3,
                basicBeatmapEventData.time,
                TechnicolorConfig.Instance.TechnicolorLightsStyle);
            particleSystem.GetParticles(particles, particles.Length);
            for (int i = 0; i < particleSystem.particleCount; i++)
            {
                particles[i].startColor = TechnicolorController.GetTechnicolor(
                    basicBeatmapEventData.value > 3,
                    basicBeatmapEventData.time + particles[i].randomSeed,
                    TechnicolorConfig.Instance.TechnicolorLightsStyle);
            }

            particleSystem.SetParticles(particles, particleSystem.particleCount);

            return false;
        }
    }
}
