namespace Technicolor.HarmonyPatches
{
    using HarmonyLib;
    using Technicolor.Settings;

    [HarmonyPatch(typeof(BeatmapObjectCallbackController))]
    [HarmonyPatch("Start")]
    internal static class BeatmapObjectCallbackControllerStart
    {
        private static void Postfix()
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorEnabled)
            {
                GradientController.InitializeGradients();
            }
        }
    }
}
