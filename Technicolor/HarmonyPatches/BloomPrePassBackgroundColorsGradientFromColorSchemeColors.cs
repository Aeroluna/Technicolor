namespace Technicolor.HarmonyPatches
{
    using Heck;
    using UnityEngine;

    [HeckPatch(typeof(BloomPrePassBackgroundColorsGradientFromColorSchemeColors))]
    [HeckPatch("Start")]
    [HeckPatch((int)TechniPatchType.LIGHTS)]
    internal class BloomPrePassBackgroundColorsGradientFromColorSchemeColorsStart
    {
        private static BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[]? _elements;
        private static BloomPrePassBackgroundColorsGradient? _bloomPrePassBackgroundColorsGradient;

        internal static void SetGradientColors(Color colorLeft, Color colorRight)
        {
            if (_bloomPrePassBackgroundColorsGradient != null && _elements != null)
            {
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
        }

        private static void Postfix(BloomPrePassBackgroundColorsGradient ____bloomPrePassBackgroundColorsGradient, BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[] ____elements)
        {
            _bloomPrePassBackgroundColorsGradient = ____bloomPrePassBackgroundColorsGradient;
            _elements = ____elements;

            if (Settings.TechnicolorConfig.Instance.DisableGradientBackground)
            {
                SetGradientColors(Color.black, Color.black);
            }
        }
    }
}
