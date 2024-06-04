using Heck;
using Technicolor.Managers;
using Technicolor.Settings;
using static Technicolor.TechnicolorController;

namespace Technicolor
{
    [Module("Technicolor", 1, LoadType.Active, new[] { "ChromaColorizer" }, new[] { "Chroma" })]
    internal class TechnicolorModule : IModule
    {
        private readonly Config _config;

        private TechnicolorModule(Config config)
        {
            _config = config;
        }

        [ModuleCondition]
        private bool Condition()
        {
            return _config.TechnicolorEnabled;
        }

        [ModuleCallback]
        private void Callback(bool value)
        {
            TechnicolorEnabled = value;

            if (value)
            {
                LightsEnabled = _config.TechnicolorLightsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                ObstaclesEnabled = _config.TechnicolorWallsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                NotesEnabled = _config.TechnicolorBlocksStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT) ||
                               _config is { UseLeftBlocksStyle: true, LeftTechnicolorBlocksStyle: not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT) };
                BombsEnabled = _config.TechnicolorBombsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
            }
            else
            {
                LightsEnabled = false;
                ObstaclesEnabled = false;
                NotesEnabled = false;
                BombsEnabled = false;
            }
        }
    }
}
