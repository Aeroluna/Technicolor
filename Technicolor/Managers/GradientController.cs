using System;
using Chroma.Colorizer;
using JetBrains.Annotations;
using Technicolor.HarmonyPatches;
using Technicolor.Settings;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Technicolor.Managers
{
    public enum TechnicolorStyle
    {
        OFF,
        WARM_COLD,
        PURE_RANDOM,
        GRADIENT
    }

    public enum TechnicolorTransition
    {
        FLAT,
        SMOOTH
    }

    public enum TechnicolorLightsGrouping
    {
        STANDARD,
        ISOLATED_GROUP,
        ISOLATED
    }

    [UsedImplicitly]
    internal class GradientController : ITickable
    {
        private readonly Color?[] _rainbowSaberColors = { null, null };
        private readonly bool _match;
        private readonly float _mismatchSpeedOffset;
        private readonly float _timeMult;
        private readonly float _timeGlobalMult;

        private readonly SaberColorizerManager _saberColorizerManager;
        private readonly LightColorizerManager _lightColorizerManager;
        private readonly ObstacleColorizerManager _obstacleColorizerManager;
        private readonly NoteColorizerManager _noteColorizerManager;
        private readonly BombColorizerManager _bombColorizerManager;
        private readonly BackgroundGradientColorizer _backgroundGradientColorizer;

        private readonly Color[] _leftSaberPalette = Array.Empty<Color>();
        private readonly Color[] _rightSaberPalette = Array.Empty<Color>();

        private Color _gradientColor;
        private Color _gradientLeftColor;
        private Color _gradientRightColor;

        private float _lastTime;
        private float _h;

        private GradientController(
            SaberColorizerManager saberColorizerManager,
            LightColorizerManager lightColorizerManager,
            ObstacleColorizerManager obstacleColorizerManager,
            NoteColorizerManager noteColorizerManager,
            BombColorizerManager bombColorizerManager,
            BackgroundGradientColorizer backgroundGradientColorizer)
        {
            _saberColorizerManager = saberColorizerManager;
            _lightColorizerManager = lightColorizerManager;
            _obstacleColorizerManager = obstacleColorizerManager;
            _noteColorizerManager = noteColorizerManager;
            _bombColorizerManager = bombColorizerManager;
            _backgroundGradientColorizer = backgroundGradientColorizer;
            _match = TechnicolorConfig.Instance.Desync;
            _mismatchSpeedOffset = _match ? 0 : 0.5f;
            _timeMult = TechnicolorConfig.Instance.TechnicolorLightsFrequency;
            _timeGlobalMult = (TechnicolorConfig.Instance.TechnicolorLightsFrequency / 2) + 0.7f;

            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorLightsStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowLights;
            }

            if (config.TechnicolorLightsStyle != TechnicolorStyle.OFF && !config.DisableGradientBackground)
            {
                UpdateTechnicolourEvent += RainbowGradientBackground;
            }

            if (config.TechnicolorBlocksStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowNotes;
            }

            if (config.TechnicolorWallsStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowWalls;
            }

            if (config.TechnicolorBombsStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowBombs;
            }

            // sabers use this script regardless of technicolour style
            if (config.TechnicolorSabersStyle == TechnicolorStyle.OFF)
            {
                return;
            }

            switch (config.TechnicolorSabersStyle)
            {
                case TechnicolorStyle.GRADIENT:
                    UpdateTechnicolourEvent += GradientTick;
                    break;

                case TechnicolorStyle.PURE_RANDOM:
                    _leftSaberPalette = new[] { Color.HSVToRGB(Random.value, 1f, 1f), Color.HSVToRGB(Random.value, 1f, 1f) };
                    _rightSaberPalette = new[] { Color.HSVToRGB(Random.value, 1f, 1f), Color.HSVToRGB(Random.value, 1f, 1f) };
                    UpdateTechnicolourEvent += RandomTick;
                    break;

                default:
                    _leftSaberPalette = TechnicolorController.TechnicolorWarmPalette;
                    _rightSaberPalette = TechnicolorController.TechnicolorColdPalette;
                    UpdateTechnicolourEvent += PaletteTick;
                    break;
            }

            UpdateTechnicolourEvent += RainbowSabers;
        }

        private event Action? UpdateTechnicolourEvent;

        private Color[] LeftSaberPalette => _leftSaberPalette ?? throw new InvalidOperationException($"[{nameof(_leftSaberPalette)}] was null.");

        private Color[] RightSaberPalette => _rightSaberPalette ?? throw new InvalidOperationException($"[{nameof(_rightSaberPalette)}] was null.");

        public void Tick()
        {
            _gradientColor = Color.HSVToRGB(Mathf.Repeat(Time.time * _timeGlobalMult, 1f), 1f, 1f);
            _gradientLeftColor = Color.HSVToRGB(Mathf.Repeat((Time.time * _timeMult) + _mismatchSpeedOffset, 1f), 1f, 1f);
            _gradientRightColor = Color.HSVToRGB(Mathf.Repeat(Time.time * _timeMult, 1f), 1f, 1f);

            UpdateTechnicolourEvent?.Invoke();
        }

        private void RainbowLights()
        {
            if (TechnicolorConfig.Instance.TechnicolorLightsGrouping == TechnicolorLightsGrouping.ISOLATED)
            {
                _lightColorizerManager.GlobalColorize(false, _gradientLeftColor, _gradientRightColor, _gradientLeftColor, _gradientRightColor);
                foreach (LightColorizer lightColorizer in _lightColorizerManager.Colorizers.Values)
                {
                    foreach (ILightWithId light in lightColorizer.Lights)
                    {
                        float seed = Math.Abs(light.GetHashCode()) % 1000;
                        seed *= 0.001f;
                        Color colorLeft = Color.HSVToRGB(Mathf.Repeat((Time.time * _timeMult) + _mismatchSpeedOffset + seed, 1f), 1f, 1f);
                        Color colorRight = Color.HSVToRGB(Mathf.Repeat((Time.time * _timeMult) + seed, 1f), 1f, 1f);
                        lightColorizer.Colorize(new[] { light }, colorLeft, colorRight, colorLeft, colorRight);
                    }
                }
            }
            else
            {
                _lightColorizerManager.GlobalColorize(true, _gradientLeftColor, _gradientRightColor, _gradientLeftColor, _gradientRightColor);
            }
        }

        private void RainbowGradientBackground()
        {
            _backgroundGradientColorizer.SetGradientColors(_gradientLeftColor, _gradientRightColor);
        }

        private void RainbowNotes()
        {
            _noteColorizerManager.GlobalColorize(_gradientLeftColor, ColorType.ColorA);
            _noteColorizerManager.GlobalColorize(_gradientRightColor, ColorType.ColorB);
        }

        private void RainbowWalls()
        {
            _obstacleColorizerManager.GlobalColorize(_gradientColor);
        }

        private void RainbowBombs()
        {
            _bombColorizerManager.GlobalColorize(_gradientColor);
        }

        private void RainbowSabers()
        {
            _saberColorizerManager.GlobalColorize(_rainbowSaberColors[0], SaberType.SaberA);
            _saberColorizerManager.GlobalColorize(_rainbowSaberColors[1], SaberType.SaberB);
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

            _rainbowSaberColors[0] = Color.Lerp(_leftSaberPalette[0], _leftSaberPalette[1], _h);
            _rainbowSaberColors[1] = Color.Lerp(_rightSaberPalette[0], _rightSaberPalette[1], _h);
            _lastTime = Time.time;
        }

        private void RandomCycleNext()
        {
            _leftSaberPalette[0] = _leftSaberPalette[1];
            _rightSaberPalette[0] = _rightSaberPalette[1];
            _leftSaberPalette[1] = Color.HSVToRGB(Random.value, 1f, 1f);
            if (_match)
            {
                _rightSaberPalette[1] = _leftSaberPalette[1];
            }
            else
            {
                _rightSaberPalette[1] = Color.HSVToRGB(Random.value, 1f, 1f);
            }
        }
    }
}
