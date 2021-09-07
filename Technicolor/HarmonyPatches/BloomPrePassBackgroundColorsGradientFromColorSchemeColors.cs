namespace Technicolor.HarmonyPatches
{
    using Heck;
    using UnityEngine;

    [HeckPatch(typeof(BloomPrePassBackgroundColorsGradientFromColorSchemeColors))]
    [HeckPatch("Start")]
    [HeckPatch((int)TechniPatchType.LIGHTS)]
    internal class BloomPrePassBackgroundColorsGradientFromColorSchemeColorsStart
    {
        private static BloomPrePassBackgroundColorsGradient? _bloomPrePassBackgroundColorsGradient;
        private static float _skyColorIntensity = 0.1f;
        private static float _groundColorIntensity = 0.1f;

        internal static void SetGradientColors(Color colorLeft, Color colorRight)
        {
            if (!Settings.TechnicolorConfig.Instance!.DisableGradientBackground && _bloomPrePassBackgroundColorsGradient != null)
            {
                _bloomPrePassBackgroundColorsGradient.elements[0].color = colorLeft * _groundColorIntensity;
                _bloomPrePassBackgroundColorsGradient.elements[1].color = colorLeft * _groundColorIntensity;
                _bloomPrePassBackgroundColorsGradient.elements[5].color = colorRight * _skyColorIntensity;
                _bloomPrePassBackgroundColorsGradient.UpdateGradientTexture();
            }
        }

        private static void Postfix(BloomPrePassBackgroundColorsGradient ____bloomPrePassBackgroundColorsGradient, float ____skyColorIntensity, float ____groundColorIntensity)
        {
            if (Settings.TechnicolorConfig.Instance!.DisableGradientBackground)
            {
                ____bloomPrePassBackgroundColorsGradient.elements[0].color = Color.black;
                ____bloomPrePassBackgroundColorsGradient.elements[1].color = Color.black;
                ____bloomPrePassBackgroundColorsGradient.elements[5].color = Color.black;
                ____bloomPrePassBackgroundColorsGradient.UpdateGradientTexture();
            }
            else
            {
                _bloomPrePassBackgroundColorsGradient = ____bloomPrePassBackgroundColorsGradient;
                _skyColorIntensity = ____skyColorIntensity;
                _groundColorIntensity = ____groundColorIntensity;
            }
        }
    }
}
