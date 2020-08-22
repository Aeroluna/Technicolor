namespace Technicolor.HarmonyPatches
{
    using System;
    using Chroma;
    using HarmonyLib;
    using Technicolor.Settings;

    [HarmonyPatch(
        typeof(StandardLevelScenesTransitionSetupDataSO),
        new Type[] { typeof(IDifficultyBeatmap), typeof(OverrideEnvironmentSettings), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(string), typeof(bool) })]
    [HarmonyPatch("Init")]
    [HarmonyAfter(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
    internal static class StandardLevelScenesTransitionSetupDataSOInit
    {
        private static void Prefix()
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorEnabled && !ChromaController.ChromaIsActive)
            {
                TechnicolorController.ToggleTechniPatches(config.TechnicolorLightsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorLightsStyle != TechnicolorStyle.OFF, TechniPatchType.LIGHTS);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorWallsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorWallsStyle != TechnicolorStyle.OFF, TechniPatchType.OBSTACLES);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorBlocksStyle != TechnicolorStyle.GRADIENT && config.TechnicolorBlocksStyle != TechnicolorStyle.OFF, TechniPatchType.NOTES);
                TechnicolorController.ToggleTechniPatches(config.TechnicolorBombsStyle != TechnicolorStyle.GRADIENT && config.TechnicolorBombsStyle != TechnicolorStyle.OFF, TechniPatchType.BOMBS);

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
