using HarmonyLib;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    [HarmonyPatch("ColorWasSet")]
    internal static class ColorBooster
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BloomPrePassBackgroundLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(LightWithIds.LightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(InstancedMaterialLightWithId), "ColorWasSet")]
        private static void BoostNewColor(ref Color newColor)
        {
            BoostColor(ref newColor);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DirectionalLightWithIds), "ColorWasSet")]
        [HarmonyPatch(typeof(EnableRendererWithLightId), "ColorWasSet")]
        [HarmonyPatch(typeof(MaterialLightWithIds), "ColorWasSet")]
        [HarmonyPatch(typeof(ParticleSystemLightWithIds), "ColorWasSet")]
        [HarmonyPatch(typeof(SpriteLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(UnityLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(RectangleFakeGlowLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(SongTimeSyncedVideoPlayer), "ColorWasSet")]
        [HarmonyPatch(typeof(SpawnRotationChevron), "ColorWasSet")]
        [HarmonyPatch(typeof(BeatLine), "ColorWasSet")]
        [HarmonyPatch(typeof(TubeBloomPrePassLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(PointLightWithIds), "ColorWasSet")]
        [HarmonyPatch(typeof(ParticleSystemLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(MaterialLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(DirectionalLightWithLightGroupIds), "ColorWasSet")]
        [HarmonyPatch(typeof(DirectionalLightWithId), "ColorWasSet")]
        [HarmonyPatch(typeof(BloomPrePassBackgroundColorsGradientTintColorWithLightIds), "ColorWasSet")]
        [HarmonyPatch(typeof(BloomPrePassBackgroundColorsGradientTintColorWithLightId), "ColorWasSet")]
        [HarmonyPatch(typeof(BloomPrePassBackgroundColorsGradientElementWithLightId), "ColorWasSet")]
        private static void BoostColor(ref Color color)
        {
            float mult = 1 + TechnicolorConfig.Instance.ColorBoost;
            color.a *= mult;
        }
    }
}
