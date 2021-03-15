namespace Technicolor.HarmonyPatches
{
    using IPA.Utilities;
    using Technicolor.Settings;
    using UnityEngine;

    [TechniPatch(typeof(ParticleSystemEventEffect))]
    [TechniPatch("HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger")]
    [TechniPatch(TechniPatchType.LIGHTS)]
    internal class ParticleSystemEventEffectHandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger
    {
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem.MainModule>.Accessor _mainModuleAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem.MainModule>.GetAccessor("_mainModule");
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem.Particle[]>.Accessor _particlesAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem.Particle[]>.GetAccessor("_particles");
        private static readonly FieldAccessor<ParticleSystemEventEffect, ParticleSystem>.Accessor _particleSystemAccessor = FieldAccessor<ParticleSystemEventEffect, ParticleSystem>.GetAccessor("_particleSystem");

        private static bool Prefix(ParticleSystemEventEffect __instance, BeatmapEventData beatmapEventData, BeatmapEventType ____colorEvent)
        {
            if (TechnicolorConfig.Instance.TechnicolorEnabled && beatmapEventData.type == ____colorEvent &&
                beatmapEventData.value > 0 && beatmapEventData.value <= 7)
            {
                if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED &&
                    TechnicolorController.TechniLightRandom.NextDouble() < TechnicolorConfig.Instance.TechnicolorLightsFrequency)
                {
                    ParticleSystem.MainModule mainmodule = _mainModuleAccessor(ref __instance);
                    ParticleSystem.Particle[] particles = _particlesAccessor(ref __instance);
                    ParticleSystem particleSystem = _particleSystemAccessor(ref __instance);
                    mainmodule.startColor = TechnicolorController.GetTechnicolor(beatmapEventData.value > 3, beatmapEventData.time, TechnicolorConfig.Instance.TechnicolorLightsStyle);
                    particleSystem.GetParticles(particles, particles.Length);
                    for (int i = 0; i < particleSystem.particleCount; i++)
                    {
                        particles[i].startColor = TechnicolorController.GetTechnicolor(beatmapEventData.value > 3, beatmapEventData.time + particles[i].randomSeed, TechnicolorConfig.Instance.TechnicolorLightsStyle);
                    }

                    particleSystem.SetParticles(particles, particleSystem.particleCount);

                    return false;
                }
            }

            return true;
        }
    }
}
