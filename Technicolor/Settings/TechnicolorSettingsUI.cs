using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Util;
using JetBrains.Annotations;
using Technicolor.Managers;

namespace Technicolor.Settings
{
    internal class TechnicolorSettingsUI : PersistentSingleton<TechnicolorSettingsUI>
    {
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

#pragma warning disable CA1822
        [UsedImplicitly]
        [UIValue("technicolor")]
        public bool TechnicolorEnabled
        {
            get => TechnicolorConfig.Instance.TechnicolorEnabled;
            set => TechnicolorConfig.Instance.TechnicolorEnabled = value;
        }

        [UsedImplicitly]
        [UIValue("techlights")]
        public TechnicolorStyle TechnicolorLightsStyle
        {
            get => TechnicolorConfig.Instance.TechnicolorLightsStyle;
            set => TechnicolorConfig.Instance.TechnicolorLightsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lightsgroup")]
        public TechnicolorLightsGrouping TechnicolorLightsGroup
        {
            get => TechnicolorConfig.Instance.TechnicolorLightsGrouping;
            set => TechnicolorConfig.Instance.TechnicolorLightsGrouping = value;
        }

        [UsedImplicitly]
        [UIValue("lightsfreq")]
        public float TechnicolorLightsFrequency
        {
            get => TechnicolorConfig.Instance.TechnicolorLightsFrequency;
            set => TechnicolorConfig.Instance.TechnicolorLightsFrequency = value;
        }

        [UsedImplicitly]
        [UIValue("techbarriers")]
        public TechnicolorStyle TechnicolorWallsStyle
        {
            get => TechnicolorConfig.Instance.TechnicolorWallsStyle;
            set => TechnicolorConfig.Instance.TechnicolorWallsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("techbombs")]
        public TechnicolorStyle TechnicolorBombsStyle
        {
            get => TechnicolorConfig.Instance.TechnicolorBombsStyle;
            set => TechnicolorConfig.Instance.TechnicolorBombsStyle = value;
        }

        [UsedImplicitly]
        [UIValue("technotes")]
        public TechnicolorStyle TechnicolorBlocksStyle
        {
            get => TechnicolorConfig.Instance.TechnicolorBlocksStyle;
            set => TechnicolorConfig.Instance.TechnicolorBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("techsabers")]
        public TechnicolorStyle TechnicolorSabersStyle
        {
            get => TechnicolorConfig.Instance.TechnicolorSabersStyle;
            set => TechnicolorConfig.Instance.TechnicolorSabersStyle = value;
        }

        [UsedImplicitly]
        [UIValue("desync")]
        public bool Desync
        {
            get => !TechnicolorConfig.Instance.Desync;
            set => TechnicolorConfig.Instance.Desync = !value;
        }

        [UsedImplicitly]
        [UIValue("disablegradient")]
        public bool DisableGradientBackground
        {
            get => TechnicolorConfig.Instance.DisableGradientBackground;
            set => TechnicolorConfig.Instance.DisableGradientBackground = value;
        }

        [UsedImplicitly]
        [UIValue("colorboost")]
        public float ColorBoost
        {
            get => TechnicolorConfig.Instance.ColorBoost;
            set => TechnicolorConfig.Instance.ColorBoost = value;
        }

        [UsedImplicitly]
        [UIValue("useleftnote")]
        public bool UseLeftBlocksStyle
        {
            get => TechnicolorConfig.Instance.UseLeftBlocksStyle;
            set => TechnicolorConfig.Instance.UseLeftBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lefttechnotes")]
        public TechnicolorStyle LeftTechnicolorBlocksStyle
        {
            get => TechnicolorConfig.Instance.LeftTechnicolorBlocksStyle;
            set => TechnicolorConfig.Instance.LeftTechnicolorBlocksStyle = value;
        }

        [UsedImplicitly]
        [UIValue("useleftsaber")]
        public bool UseLeftSaberStyle
        {
            get => TechnicolorConfig.Instance.UseLeftSaberStyle;
            set => TechnicolorConfig.Instance.UseLeftSaberStyle = value;
        }

        [UsedImplicitly]
        [UIValue("lefttechsabers")]
        public TechnicolorStyle LeftTechnicolorSabersStyle
        {
            get => TechnicolorConfig.Instance.LeftTechnicolorSabersStyle;
            set => TechnicolorConfig.Instance.LeftTechnicolorSabersStyle = value;
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
