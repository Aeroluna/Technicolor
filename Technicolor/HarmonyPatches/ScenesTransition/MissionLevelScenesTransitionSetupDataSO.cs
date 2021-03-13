namespace Technicolor.HarmonyPatches
{
    using HarmonyLib;

    [HarmonyPatch(
        typeof(MissionLevelScenesTransitionSetupDataSO),
        new[] { typeof(string), typeof(IDifficultyBeatmap), typeof(IPreviewBeatmapLevel), typeof(MissionObjective[]), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(string) })]
    [HarmonyPatch("Init")]
    [HarmonyAfter("com.noodle.BeatSaber.ChromaCore")]
    internal static class MissionLevelScenesTransitionSetupDataSOInit
    {
        private static void Prefix()
        {
            SceneTransitionHelper.Patch();
        }
    }
}