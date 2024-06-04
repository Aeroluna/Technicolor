using SiraUtil.Affinity;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    internal class ColorBooster : IAffinity
    {
        private readonly Config _config;

        internal ColorBooster(Config config)
        {
            _config = config;
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(BloomPrePassBackgroundLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(LightWithIds.LightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(InstancedMaterialLightWithId), "ColorWasSet")]
        private void BoostNewColor(ref Color newColor)
        {
            BoostColor(ref newColor);
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(DirectionalLightWithIds), "ColorWasSet")]
        [AffinityPatch(typeof(EnableRendererWithLightId), "ColorWasSet")]
        [AffinityPatch(typeof(MaterialLightWithIds), "ColorWasSet")]
        [AffinityPatch(typeof(ParticleSystemLightWithIds), "ColorWasSet")]
        [AffinityPatch(typeof(SpriteLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(UnityLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(RectangleFakeGlowLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(SongTimeSyncedVideoPlayer), "ColorWasSet")]
        [AffinityPatch(typeof(SpawnRotationChevron), "ColorWasSet")]
        [AffinityPatch(typeof(BeatLine), "ColorWasSet")]
        [AffinityPatch(typeof(TubeBloomPrePassLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(PointLightWithIds), "ColorWasSet")]
        [AffinityPatch(typeof(ParticleSystemLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(MaterialLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(DirectionalLightWithLightGroupIds), "ColorWasSet")]
        [AffinityPatch(typeof(DirectionalLightWithId), "ColorWasSet")]
        [AffinityPatch(typeof(BloomPrePassBackgroundColorsGradientTintColorWithLightIds), "ColorWasSet")]
        [AffinityPatch(typeof(BloomPrePassBackgroundColorsGradientTintColorWithLightId), "ColorWasSet")]
        [AffinityPatch(typeof(BloomPrePassBackgroundColorsGradientElementWithLightId), "ColorWasSet")]
        private void BoostColor(ref Color color)
        {
            float mult = 1 + _config.ColorBoost;
            color.a *= mult;
        }
    }
}
