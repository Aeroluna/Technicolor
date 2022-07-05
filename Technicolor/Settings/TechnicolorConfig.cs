using System;
using JetBrains.Annotations;
using Technicolor.Managers;

namespace Technicolor.Settings
{
    [UsedImplicitly]
    public class TechnicolorConfig
    {
        private static TechnicolorConfig? _instance;

        public static TechnicolorConfig Instance
        {
            get => _instance ?? throw new InvalidOperationException("TechnicolorConfig instance not yet created.");
            set => _instance = value;
        }

        public bool TechnicolorEnabled { get; set; } = true;

        public TechnicolorStyle TechnicolorLightsStyle { get; set; } = TechnicolorStyle.GRADIENT;

        public TechnicolorLightsGrouping TechnicolorLightsGrouping { get; set; } = TechnicolorLightsGrouping.ISOLATED;

        public float TechnicolorLightsFrequency { get; set; } = 0.1f;

        public TechnicolorStyle TechnicolorSabersStyle { get; set; } = TechnicolorStyle.OFF;

        public TechnicolorStyle TechnicolorBlocksStyle { get; set; } = TechnicolorStyle.OFF;

        public TechnicolorStyle TechnicolorWallsStyle { get; set; } = TechnicolorStyle.GRADIENT;

        public TechnicolorStyle TechnicolorBombsStyle { get; set; } = TechnicolorStyle.PURE_RANDOM;

        public bool Desync { get; set; }

        public bool DisableGradientBackground { get; set; }

        public float ColorBoost { get; set; }
    }
}
