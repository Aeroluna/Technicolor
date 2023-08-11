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

        private readonly bool _randomLeft;
        private readonly bool _randomRight;

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

            bool useSameBlocksStyle = !config.UseLeftBlocksStyle;
            bool leftBlocksAdded = false;
            if (config.LeftTechnicolorBlocksStyle == TechnicolorStyle.GRADIENT && !useSameBlocksStyle)
            {
                UpdateTechnicolourEvent += RainbowNotesLeft;
                leftBlocksAdded = true;
            }

            if (config.TechnicolorBlocksStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowNotesRight;
                if (useSameBlocksStyle && !leftBlocksAdded)
                {
                    UpdateTechnicolourEvent += RainbowNotesLeft;
                }
            }

            if (config.TechnicolorWallsStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowWalls;
            }

            if (config.TechnicolorBombsStyle == TechnicolorStyle.GRADIENT)
            {
                UpdateTechnicolourEvent += RainbowBombs;
            }

            bool useSameSaberStyle = !config.UseLeftSaberStyle;
            bool randomAdded = false;
            bool leftAdded = false;

            if (config.LeftTechnicolorSabersStyle != TechnicolorStyle.OFF && !useSameSaberStyle)
            {
                switch (config.LeftTechnicolorSabersStyle)
                {
                    case TechnicolorStyle.GRADIENT:
                        UpdateTechnicolourEvent += GradientTickLeft;

                        break;

                    case TechnicolorStyle.PURE_RANDOM:
                        _leftSaberPalette = new[] { Color.HSVToRGB(Random.value, 1f, 1f), Color.HSVToRGB(Random.value, 1f, 1f) };
                        _randomLeft = true;
                        UpdateTechnicolourEvent += RandomTick;
                        randomAdded = true;

                        break;

                    default:
                        _leftSaberPalette = TechnicolorController.TechnicolorWarmPalette;
                        UpdateTechnicolourEvent += PaletteTickLeft;

                        break;
                }

                UpdateTechnicolourEvent += RainbowSaberLeft;
                leftAdded = true;
            }

            // sabers use this script regardless of technicolour style
            if (config.TechnicolorSabersStyle == TechnicolorStyle.OFF)
            {
                return;
            }

            switch (config.TechnicolorSabersStyle)
            {
                case TechnicolorStyle.GRADIENT:
                    UpdateTechnicolourEvent += GradientTickRight;
                    if (useSameSaberStyle)
                    {
                        UpdateTechnicolourEvent += GradientTickLeft;
                    }

                    break;

                case TechnicolorStyle.PURE_RANDOM:
                    _rightSaberPalette = new[] { Color.HSVToRGB(Random.value, 1f, 1f), Color.HSVToRGB(Random.value, 1f, 1f) };
                    _randomRight = true;

                    if (useSameSaberStyle)
                    {
                        _leftSaberPalette = new[] { Color.HSVToRGB(Random.value, 1f, 1f), Color.HSVToRGB(Random.value, 1f, 1f) };
                        _randomLeft = true;
                    }

                    if (!randomAdded)
                    {
                        UpdateTechnicolourEvent += RandomTick;
                    }

                    break;

                default:
                    _rightSaberPalette = TechnicolorController.TechnicolorColdPalette;
                    UpdateTechnicolourEvent += PaletteTickRight;

                    if (useSameSaberStyle)
                    {
                        _leftSaberPalette = TechnicolorController.TechnicolorWarmPalette;
                        UpdateTechnicolourEvent += PaletteTickLeft;
                    }

                    break;
            }

            UpdateTechnicolourEvent += RainbowSaberRight;
            if (useSameSaberStyle && !leftAdded)
            {
                UpdateTechnicolourEvent += RainbowSaberLeft;
            }
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

        private void RainbowNotesLeft()
        {
            _noteColorizerManager.GlobalColorize(_gradientLeftColor, ColorType.ColorA);
        }

        private void RainbowNotesRight()
        {
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

        private void RainbowSaberLeft()
        {
            _saberColorizerManager.GlobalColorize(_rainbowSaberColors[1], SaberType.SaberA);
        }

        private void RainbowSaberRight()
        {
            _saberColorizerManager.GlobalColorize(_rainbowSaberColors[0], SaberType.SaberB);
        }

        /*
         * PALETTED
         */

        private void PaletteTickLeft()
        {
            _rainbowSaberColors[1] = TechnicolorController.GetLerpedFromArray(LeftSaberPalette, Time.time + _mismatchSpeedOffset);
        }

        private void PaletteTickRight()
        {
            _rainbowSaberColors[0] = TechnicolorController.GetLerpedFromArray(RightSaberPalette, Time.time);
        }

        private void GradientTickLeft()
        {
            _rainbowSaberColors[1] = _gradientLeftColor;
        }

        private void GradientTickRight()
        {
            _rainbowSaberColors[0] = _gradientRightColor;
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
                if (_randomLeft)
                {
                    _leftSaberPalette[0] = _leftSaberPalette[1];
                    _leftSaberPalette[1] = Color.HSVToRGB(Random.value, 1f, 1f);
                }

                if (_randomRight)
                {
                    _rightSaberPalette[0] = _rightSaberPalette[1];
                    if (_match && _randomLeft)
                    {
                        _rightSaberPalette[1] = _leftSaberPalette[1];
                    }
                    else
                    {
                        _rightSaberPalette[1] = Color.HSVToRGB(Random.value, 1f, 1f);
                    }
                }
            }

            if (_randomRight)
            {
                _rainbowSaberColors[0] = Color.Lerp(_rightSaberPalette[0], _rightSaberPalette[1], _h);
            }

            if (_randomLeft)
            {
                _rainbowSaberColors[1] = Color.Lerp(_leftSaberPalette[0], _leftSaberPalette[1], _h);
            }

            _lastTime = Time.time;
        }
    }
}
