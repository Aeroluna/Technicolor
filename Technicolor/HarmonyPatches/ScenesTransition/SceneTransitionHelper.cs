namespace Technicolor.HarmonyPatches
{
    using Chroma;
    using Technicolor.Settings;

    internal static class SceneTransitionHelper
    {
        internal static void Patch()
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorEnabled && !ChromaController.ChromaIsActive)
            {
                TechnicolorController.ToggleTechniPatches(config.TechnicolorLightsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorLightsStyle != TechnicolorStyle.OFF, TechniPatchType.LIGHTS);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorWallsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorWallsStyle != TechnicolorStyle.OFF, TechniPatchType.OBSTACLES);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorBlocksStyle != TechnicolorStyle.GRADIENT && config.TechnicolorBlocksStyle != TechnicolorStyle.OFF, TechniPatchType.NOTES);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorBombsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorBombsStyle != TechnicolorStyle.OFF, TechniPatchType.BOMBS);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorLightsStyle != TechnicolorStyle.OFF, TechniPatchType.FCKGRADIENTS);

                if (config.TechnicolorBlocksStyle != TechnicolorStyle.OFF && config.TechnicolorSabersStyle == TechnicolorStyle.OFF)
                {
                    ChromaController.DoColorizerSabers = true;
                }
            }
            else
            {
                TechnicolorController.ToggleTechniPatches(false, TechniPatchType.LIGHTS);
                TechnicolorController.ToggleTechniPatches(false, TechniPatchType.OBSTACLES);
                TechnicolorController.ToggleTechniPatches(false, TechniPatchType.NOTES);
                TechnicolorController.ToggleTechniPatches(false, TechniPatchType.BOMBS);
            }
        }
    }
}
