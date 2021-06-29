namespace Technicolor.Settings
{
    public class TechnicolorConfig
    {
        public static TechnicolorConfig? Instance { get; set; }

        public bool TechnicolorEnabled { get; set; } = true;

        public TechnicolorStyle TechnicolorLightsStyle { get; set; } = TechnicolorStyle.WARM_COLD;

        public TechnicolorLightsGrouping TechnicolorLightsGrouping { get; set; } = TechnicolorLightsGrouping.STANDARD;

        public float TechnicolorLightsFrequency { get; set; } = 0.1f;

        public TechnicolorStyle TechnicolorSabersStyle { get; set; } = TechnicolorStyle.OFF;

        public TechnicolorStyle TechnicolorBlocksStyle { get; set; } = TechnicolorStyle.OFF;

        public TechnicolorStyle TechnicolorWallsStyle { get; set; } = TechnicolorStyle.GRADIENT;

        public TechnicolorStyle TechnicolorBombsStyle { get; set; } = TechnicolorStyle.PURE_RANDOM;

        public bool Desync { get; set; } = false;

        public bool DisableGradientBackground { get; set; } = false;
    }
}
