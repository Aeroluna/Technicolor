namespace Technicolor.HarmonyPatches
{
    using Chroma;
    using HarmonyLib;
    using Technicolor.Settings;

    [HarmonyPatch(typeof(BeatmapObjectCallbackController))]
    [HarmonyPatch("Start")]
    internal static class BeatmapObjectCallbackControllerStart
    {
        private static void Postfix()
        {
            if (TechnicolorConfig.Instance.TechnicolorEnabled && !ChromaController.ChromaIsActive)
            {
                GradientController.InitializeGradients();
            }
        }
    }
}
