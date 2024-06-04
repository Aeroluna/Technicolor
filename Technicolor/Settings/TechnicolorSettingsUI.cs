using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;
using JetBrains.Annotations;
using Technicolor.Managers;

namespace Technicolor.Settings
{
    internal class TechnicolorSettingsUI : IDisposable
    {
        private readonly Config _config;

        [UsedImplicitly]
        [UIValue("techlightschoices")]
        private readonly List<object> _techlightsChoices = new() { TechnicolorStyle.OFF, TechnicolorStyle.WARM_COLD, TechnicolorStyle.PURE_RANDOM, TechnicolorStyle.GRADIENT };

        [UsedImplicitly]
        [UIValue("techbarrierschoices")]
        private readonly List<object> _techbarrierschoices = new() { TechnicolorStyle.OFF, TechnicolorStyle.PURE_RANDOM, TechnicolorStyle.GRADIENT };

        [UsedImplicitly]
        [UIValue("lightsgroupchoices")]
        private readonly List<object> _lightsgroupChoices = new() { TechnicolorLightsGrouping.STANDARD, TechnicolorLightsGrouping.ISOLATED_GROUP, TechnicolorLightsGrouping.ISOLATED };

        [UsedImplicitly]
        [UIValue("lightsfreqchoices")]
        private readonly List<object> _lightsfreqChoices = new() { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f, 0.55f, 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f };

        [UsedImplicitly]
        [UIValue("colorboostchoices")]
        private readonly List<object> _colorboostChoices = new() { -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f, -0.2f, -0.1f, 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2f, 2.5f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 20f, 100f };

        private TechnicolorSettingsUI(Config config)
        {
            _config = config;
            BSMLSettings.instance.AddSettingsMenu("Technicolor", "Technicolor.Settings.settings.bsml", this);
            GameplaySetup.instance.AddTab("Technicolor", "Technicolor.Settings.modifiers.bsml", this);
        }

        public void Dispose()
        {
            BSMLSettings.instance.RemoveSettingsMenu(this);
            GameplaySetup.instance.RemoveTab("Technicolor");
        }

#pragma warning disable CA1822
        [UsedImplicitly]
        [UIValue("technicolor")]
#pragma warning disable SA1201
        public bool TechnicolorEnabled
#pragma warning restore SA1201
        {
            get => _config.TechnicolorEnabled;
            set => _config.TechnicolorEnabled = value;
        }

        [UsedImplicitly]
        [UIValue("techlights")]
        public TechnicolorStyle TechnicolorLightsStyle
        {
            get => _config.TechnicolorLightsStyle;
            set => _config.TechnicolorLightsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lightsgroup")]
        public TechnicolorLightsGrouping TechnicolorLightsGroup
        {
            get => _config.TechnicolorLightsGrouping;
            set => _config.TechnicolorLightsGrouping = value;
        }

        [UsedImplicitly]
        [UIValue("lightsfreq")]
        public float TechnicolorLightsFrequency
        {
            get => _config.TechnicolorLightsFrequency;
            set => _config.TechnicolorLightsFrequency = value;
        }

        [UsedImplicitly]
        [UIValue("techbarriers")]
        public TechnicolorStyle TechnicolorWallsStyle
        {
            get => _config.TechnicolorWallsStyle;
            set => _config.TechnicolorWallsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("techbombs")]
        public TechnicolorStyle TechnicolorBombsStyle
        {
            get => _config.TechnicolorBombsStyle;
            set => _config.TechnicolorBombsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("technotes")]
        public TechnicolorStyle TechnicolorBlocksStyle
        {
            get => _config.TechnicolorBlocksStyle;
            set => _config.TechnicolorBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("techsabers")]
        public TechnicolorStyle TechnicolorSabersStyle
        {
            get => _config.TechnicolorSabersStyle;
            set => _config.TechnicolorSabersStyle = value;
        }

        [UsedImplicitly]
        [UIValue("desync")]
        public bool Desync
        {
            get => !_config.Desync;
            set => _config.Desync = !value;
        }

        [UsedImplicitly]
        [UIValue("disablegradient")]
        public bool DisableGradientBackground
        {
            get => _config.DisableGradientBackground;
            set => _config.DisableGradientBackground = value;
        }

        [UsedImplicitly]
        [UIValue("colorboost")]
        public float ColorBoost
        {
            get => _config.ColorBoost;
            set => _config.ColorBoost = value;
        }

        [UsedImplicitly]
        [UIValue("useleftnote")]
        public bool UseLeftBlocksStyle
        {
            get => _config.UseLeftBlocksStyle;
            set => _config.UseLeftBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lefttechnotes")]
        public TechnicolorStyle LeftTechnicolorBlocksStyle
        {
            get => _config.LeftTechnicolorBlocksStyle;
            set => _config.LeftTechnicolorBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("useleftsaber")]
        public bool UseLeftSaberStyle
        {
            get => _config.UseLeftSaberStyle;
            set => _config.UseLeftSaberStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lefttechsabers")]
        public TechnicolorStyle LeftTechnicolorSabersStyle
        {
            get => _config.LeftTechnicolorSabersStyle;
            set => _config.LeftTechnicolorSabersStyle = value;
        }

        [UsedImplicitly]
        [UIAction("float")]
        private string FloatDisplay(float percent)
        {
            string result = $"{percent * 100}%";
            if (percent > 0)
            {
                result = "+" + result;
            }

            return result;
        }

        [UsedImplicitly]
        [UIAction("techlightform")]
        private string TechlightFormat(TechnicolorStyle t)
        {
            return t switch
            {
                TechnicolorStyle.GRADIENT => "Gradient",
                TechnicolorStyle.PURE_RANDOM => "True Random",
                TechnicolorStyle.WARM_COLD => "Warm/Cold",
                _ => "Off"
            };
        }

        [UsedImplicitly]
        [UIAction("techgroupform")]
        private string TechgroupingFormat(TechnicolorLightsGrouping t)
        {
            return t switch
            {
                TechnicolorLightsGrouping.ISOLATED => "Mayhem",
                TechnicolorLightsGrouping.ISOLATED_GROUP => "Isolated Event",
                _ => "Standard"
            };
        }

        [UsedImplicitly]
        [UIAction("percentfreq")]
        private string PercentfreqDisplay(float percent)
        {
            return $"{percent * 100f}%" + (Math.Abs(percent - 0.1f) < 0.001 ? " (Def)" : string.Empty);
        }
#pragma warning restore CA1822
    }
}
