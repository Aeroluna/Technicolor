namespace Technicolor
{
    using System;
    using Chroma.Colorizer;
    using Technicolor.Settings;
    using UnityEngine;

    internal class GradientController : MonoBehaviour
    {
        private static GradientController? _instance;

        private readonly Color?[] _rainbowSaberColors = new Color?[] { null, null };

        private Color _gradientColor;

        private Color _gradientLeftColor;

        private Color _gradientRightColor;

        private bool _match;
        private float _mismatchSpeedOffset = 0;

        private Color[]? _leftSaberPalette;
        private Color[]? _rightSaberPalette;

        private float _lastTime = 0;
        private float _h = 0;
        private Color[] _randomCycleLeft = new Color[2];
        private Color[] _randomCycleRight = new Color[2];

        private event Action? UpdateTechnicolourEvent;

        internal static GradientController Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("Chroma_TechnicolorController");
                    _instance = gameObject.AddComponent<GradientController>();

                    _instance._match = TechnicolorConfig.Instance.Desync;
                    _instance._mismatchSpeedOffset = _instance._match ? 0 : 0.5f;
                }

                return _instance;
            }
        }

        private float TimeMult => TechnicolorConfig.Instance.TechnicolorLightsFrequency;

        private float TimeGlobalMult => (TechnicolorConfig.Instance.TechnicolorLightsFrequency / 2) + 0.7f;

        private Color[] LeftSaberPalette => _leftSaberPalette ?? throw new InvalidOperationException($"[{nameof(_leftSaberPalette)}] was null.");

        private Color[] RightSaberPalette => _rightSaberPalette ?? throw new InvalidOperationException($"[{nameof(_rightSaberPalette)}] was null.");

        internal static void InitializeGradients()
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorLightsStyle == TechnicolorStyle.GRADIENT)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowLights;
            }

            if (config.TechnicolorLightsStyle != TechnicolorStyle.OFF && !config.DisableGradientBackground)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowGradientBackground;
            }

            if (config.TechnicolorBlocksStyle == TechnicolorStyle.GRADIENT)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowNotes;
            }

            if (config.TechnicolorWallsStyle == TechnicolorStyle.GRADIENT)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowWalls;
            }

            if (config.TechnicolorBombsStyle == TechnicolorStyle.GRADIENT)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowBombs;
            }

            // sabers use this script regardless of technicolour style
            if (config.TechnicolorSabersStyle != TechnicolorStyle.OFF)
            {
                switch (config.TechnicolorSabersStyle)
                {
                    case TechnicolorStyle.GRADIENT:
                        Instance.UpdateTechnicolourEvent += Instance.GradientTick;
                        break;

                    case TechnicolorStyle.PURE_RANDOM:
                        Instance.SetupRandom();
                        Instance.UpdateTechnicolourEvent += Instance.RandomTick;
                        break;

                    case TechnicolorStyle.WARM_COLD:
                    default:
                        Instance.SetupWarmCold();
                        Instance.UpdateTechnicolourEvent += Instance.PaletteTick;
                        break;
                }

                Instance.UpdateTechnicolourEvent += Instance.RainbowSabers;
            }
        }

        private void Update()
        {
            _gradientColor = Color.HSVToRGB(Mathf.Repeat(Time.time * TimeGlobalMult, 1f), 1f, 1f);
            _gradientLeftColor = Color.HSVToRGB(Mathf.Repeat((Time.time * TimeMult) + _mismatchSpeedOffset, 1f), 1f, 1f);
            _gradientRightColor = Color.HSVToRGB(Mathf.Repeat(Time.time * TimeMult, 1f), 1f, 1f);

            UpdateTechnicolourEvent?.Invoke();
        }

        private void RainbowLights()
        {
            if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
            {
                LightColorizer.GlobalColorize(false, _gradientLeftColor, _gradientRightColor, _gradientLeftColor, _gradientRightColor);
                foreach (LightColorizer lightColorizer in LightColorizer.Colorizers.Values)
                {
                    foreach (ILightWithId light in lightColorizer.Lights)
                    {
                        float seed = Math.Abs(light.GetHashCode()) % 1000;
                        seed *= 0.001f;
                        Color colorLeft = Color.HSVToRGB(Mathf.Repeat((Time.time * TimeMult) + _mismatchSpeedOffset + seed, 1f), 1f, 1f);
                        Color colorRight = Color.HSVToRGB(Mathf.Repeat((Time.time * TimeMult) + seed, 1f), 1f, 1f);
                        lightColorizer.Colorize(new ILightWithId[] { light }, colorLeft, colorRight, colorLeft, colorRight);
                    }
                }
            }
            else
            {
                LightColorizer.GlobalColorize(true, _gradientLeftColor, _gradientRightColor, _gradientLeftColor, _gradientRightColor);
            }
        }

        private void RainbowGradientBackground()
        {
            HarmonyPatches.BloomPrePassBackgroundColorsGradientFromColorSchemeColorsStart.SetGradientColors(_gradientLeftColor, _gradientRightColor);
        }

        private void RainbowNotes()
        {
            NoteColorizer.GlobalColorize(_gradientLeftColor, ColorType.ColorA);
            NoteColorizer.GlobalColorize(_gradientRightColor, ColorType.ColorB);
        }

        private void RainbowWalls()
        {
            ObstacleColorizer.GlobalColorize(_gradientColor);
        }

        private void RainbowBombs()
        {
            BombColorizer.GlobalColorize(_gradientColor);
        }

        private void RainbowSabers()
        {
            SaberColorizer.GlobalColorize(_rainbowSaberColors[0], SaberType.SaberA);
            SaberColorizer.GlobalColorize(_rainbowSaberColors[1], SaberType.SaberB);
        }

        /*
         * PALETTED
         */

        private void PaletteTick()
        {
            _rainbowSaberColors[0] = TechnicolorController.GetLerpedFromArray(LeftSaberPalette, Time.time + _mismatchSpeedOffset);
            _rainbowSaberColors[1] = TechnicolorController.GetLerpedFromArray(RightSaberPalette, Time.time);
        }

        private void GradientTick()
        {
            _rainbowSaberColors[0] = _gradientLeftColor;
            _rainbowSaberColors[1] = _gradientRightColor;
        }

        private void SetupWarmCold()
        {
            _leftSaberPalette = TechnicolorController.TechnicolorWarmPalette;
            _rightSaberPalette = TechnicolorController.TechnicolorColdPalette;
        }

        /*
         * TRUE RANDOM
         */

        private void RandomTick()
        {
            _h += Time.time - _lastTime;
            if (_h > 1)
            {
                _h = 0;
                RandomCycleNext();
            }

            _rainbowSaberColors[0] = Color.Lerp(_randomCycleLeft[0], _randomCycleLeft[1], _h);
            _rainbowSaberColors[1] = Color.Lerp(_randomCycleRight[0], _randomCycleRight[1], _h);
            _lastTime = Time.time;
        }

        private void RandomCycleNext()
        {
            _randomCycleLeft[0] = _randomCycleLeft[1];
            _randomCycleRight[0] = _randomCycleRight[1];
            _randomCycleLeft[1] = Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f);
            if (_match)
            {
                _randomCycleRight = _randomCycleLeft;
            }
            else
            {
                _randomCycleRight[1] = Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f);
            }
        }

        private void SetupRandom()
        {
            _randomCycleLeft = new Color[] { Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f), Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f) };
            _randomCycleRight = new Color[] { Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f), Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f) };
        }
    }
}
