namespace Technicolor.HarmonyPatches
{
    using System;
    using HarmonyLib;

    [HarmonyPatch(
        typeof(StandardLevelScenesTransitionSetupDataSO),
        new Type[] { typeof(string), typeof(IDifficultyBeatmap), typeof(IPreviewBeatmapLevel), typeof(OverrideEnvironmentSettings), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(string), typeof(bool) })]
    [HarmonyPatch("Init")]
    [HarmonyAfter(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
    internal static class StandardLevelScenesTransitionSetupDataSOInit
    {
        private static void Prefix()
        {
            SceneTransitionHelper.Patch();
        }
    }
}
