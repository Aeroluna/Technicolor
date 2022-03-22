using Heck;
using Technicolor.Managers;
using Technicolor.Settings;
using static Technicolor.TechnicolorController;

namespace Technicolor
{
    internal class ModuleCallbacks
    {
        [ModuleCondition]
        private static bool Condition()
        {
            return TechnicolorConfig.Instance.TechnicolorEnabled;
        }

        [ModuleCallback]
        private static void Callback(bool value)
        {
            TechnicolorEnabled = value;

            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (value)
            {
                LightsEnabled = config.TechnicolorLightsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                ObstaclesEnabled = config.TechnicolorWallsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                NotesEnabled = config.TechnicolorBlocksStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                BombsEnabled = config.TechnicolorBombsStyle is not (TechnicolorStyle.OFF or TechnicolorStyle.GRADIENT);
                FckGradientsEnabled = config.TechnicolorLightsStyle != TechnicolorStyle.OFF;
            }
            else
            {
                LightsEnabled = false;
                ObstaclesEnabled = false;
                NotesEnabled = false;
                BombsEnabled = false;
                FckGradientsEnabled = false;
            }
        }
    }
}
