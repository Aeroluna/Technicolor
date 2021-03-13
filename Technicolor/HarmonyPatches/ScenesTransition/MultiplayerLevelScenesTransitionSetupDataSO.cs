namespace Technicolor.HarmonyPatches
{
    using System;
    using HarmonyLib;

    [HarmonyPatch(
        typeof(MultiplayerLevelScenesTransitionSetupDataSO),
        new[] { typeof(string), typeof(IPreviewBeatmapLevel), typeof(BeatmapDifficulty), typeof(BeatmapCharacteristicSO), typeof(IDifficultyBeatmap), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(bool) })]
    [HarmonyPatch("Init")]
    [HarmonyAfter("com.noodle.BeatSaber.ChromaCore")]
    internal static class MultiplayerLevelScenesTransitionSetupDataSOInit
    {
        private static void Prefix()
        {
            SceneTransitionHelper.Patch();
        }
    }
}