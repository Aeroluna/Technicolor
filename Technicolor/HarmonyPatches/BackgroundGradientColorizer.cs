using SiraUtil.Affinity;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    internal class BackgroundGradientColorizer : IAffinity
    {
        private readonly Config _config;

        private BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[]? _elements;
        private BloomPrePassBackgroundColorsGradient? _bloomPrePassBackgroundColorsGradient;

        private BackgroundGradientColorizer(Config config)
        {
            _config = config;
        }

        internal void SetGradientColors(Color colorLeft, Color colorRight)
        {
            if (_elements == null || _bloomPrePassBackgroundColorsGradient == null)
            {
                return;
            }

            int num = 0;
            while (num < _bloomPrePassBackgroundColorsGradient.elements.Length && num < _elements.Length)
            {
                if (_elements[num].loadFromColorScheme)
                {
                    switch (_elements[num].environmentColor)
                    {
                        case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0:
                        case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0Boost:
                            _elements[num].color = colorLeft * _elements[num].intensity;
                            break;

                        case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color1:
                        case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color1Boost:
                            _elements[num].color = colorRight * _elements[num].intensity;
                            break;
                    }
                }

                _bloomPrePassBackgroundColorsGradient.elements[num].color = _elements[num].color;
                num++;
            }

            _bloomPrePassBackgroundColorsGradient.UpdateGradientTexture();
        }

        // comically long class names
        [AffinityPostfix]
        [AffinityPatch(
            typeof(BloomPrePassBackgroundColorsGradientFromColorSchemeColors),
            nameof(BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Start))]
        private void Postfix(
            BloomPrePassBackgroundColorsGradientFromColorSchemeColors __instance,
            BloomPrePassBackgroundColorsGradient ____bloomPrePassBackgroundColorsGradient,
            BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[] ____elements)
        {
            _bloomPrePassBackgroundColorsGradient = ____bloomPrePassBackgroundColorsGradient;
            _elements = ____elements;

            if (_config.DisableGradientBackground)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }
}
