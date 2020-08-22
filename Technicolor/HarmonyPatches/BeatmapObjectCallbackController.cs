namespace Technicolor.HarmonyPatches
{
    using Chroma;
    using HarmonyLib;
    using Technicolor.Settings;

    [HarmonyPatch(typeof(BeatmapObjectCallbackController))]
    [HarmonyPatch("Start")]
    internal static class BeatmapObjectCallbackControllerStart
    {
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void Postfix()
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorEnabled && !ChromaController.ChromaIsActive)
            {
                GradientController.InitializeGradients();
            }
        }
    }
}
