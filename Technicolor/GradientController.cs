namespace Technicolor
{
    using System;
    using Chroma.Colorizer;
    using Technicolor.Settings;
    using UnityEngine;

    internal class GradientController : MonoBehaviour
    {
        private static GradientController _instance;

        private readonly Color?[] _rainbowSaberColors = new Color?[] { null, null };

        private Color _gradientColor;

        private Color _gradientLeftColor;

        private Color _gradientRightColor;

        private bool _match;
        private float _mismatchSpeedOffset = 0;

        private Color[] _leftSaberPalette;
        private Color[] _rightSaberPalette;

        private float _lastTime = 0;
        private float _h = 0;
        private Color[] _randomCycleLeft = new Color[2];
        private Color[] _randomCycleRight = new Color[2];

        private event Action UpdateTechnicolourEvent;

        internal static GradientController Instance
        {
            get
            {
                if (_instance == null)
                {
                    TechnicolorConfig config = TechnicolorConfig.Instance;
                    GameObject gameObject = new GameObject("Chroma_TechnicolourController");
                    _instance = gameObject.AddComponent<GradientController>();

                    _instance._match = config.Desync;
                    _instance._mismatchSpeedOffset = _instance._match ? 0 : 0.5f;
                }

                return _instance;
            }
        }

        internal static void InitializeGradients()
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorLightsStyle == TechnicolorStyle.GRADIENT)
            {
                Instance.UpdateTechnicolourEvent += Instance.RainbowLights;
            }

            if (config.TechnicolorLightsStyle != TechnicolorStyle.OFF)
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
            float timeMult = 0.1f;
            float timeGlobalMult = 0.2f;
            _gradientColor = Color.HSVToRGB(Mathf.Repeat(Time.time * timeGlobalMult, 1f), 1f, 1f);
            _gradientLeftColor = Color.HSVToRGB(Mathf.Repeat((Time.time * timeMult) + _mismatchSpeedOffset, 1f), 1f, 1f);
            _gradientRightColor = Color.HSVToRGB(Mathf.Repeat(Time.time * timeMult, 1f), 1f, 1f);

            UpdateTechnicolourEvent?.Invoke();
        }

        private void RainbowLights()
        {
            LightColorizer.SetAllLightingColors(_gradientLeftColor, _gradientRightColor);
            LightColorizer.SetAllActiveColors();
        }

        private void RainbowGradientBackground()
        {
            HarmonyPatches.BloomPrePassBackgroundColorsGradientFromColorSchemeColorsStart.SetGradientColors(_gradientLeftColor, _gradientRightColor);
        }

        private void RainbowNotes()
        {
            NoteColorizer.SetAllNoteColors(_gradientLeftColor, _gradientRightColor);
            NoteColorizer.SetAllActiveColors();
        }

        private void RainbowWalls()
        {
            ObstacleColorizer.SetAllObstacleColors(_gradientColor);
            ObstacleColorizer.SetAllActiveColors();
        }

        private void RainbowBombs()
        {
            BombColorizer.SetAllBombColors(_gradientColor);
        }

        private void RainbowSabers()
        {
            SaberColorizer.SetAllSaberColors(_rainbowSaberColors[0], _rainbowSaberColors[1]);
        }

        /*
         * PALETTED
         */

        private void PaletteTick()
        {
            _rainbowSaberColors[0] = TechnicolorController.GetLerpedFromArray(_leftSaberPalette, Time.time + _mismatchSpeedOffset);
            _rainbowSaberColors[1] = TechnicolorController.GetLerpedFromArray(_rightSaberPalette, Time.time);
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
