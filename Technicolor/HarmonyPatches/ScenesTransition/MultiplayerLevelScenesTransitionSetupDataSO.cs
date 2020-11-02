namespace Technicolor.HarmonyPatches
{
    using System;
    using HarmonyLib;

    [HarmonyPatch(
        typeof(MultiplayerLevelScenesTransitionSetupDataSO),
        new Type[] { typeof(string), typeof(IPreviewBeatmapLevel), typeof(BeatmapDifficulty), typeof(BeatmapCharacteristicSO), typeof(IDifficultyBeatmap), typeof(ColorScheme), typeof(GameplayModifiers), typeof(PlayerSpecificSettings), typeof(PracticeSettings), typeof(bool) })]
    [HarmonyPatch("Init")]
    [HarmonyAfter(new string[] { "com.noodle.BeatSaber.ChromaCore" })]
    internal static class MultiplayerLevelScenesTransitionSetupDataSOInit
    {
        private static void Prefix()
        {
            SceneTransitionHelper.Patch();
        }
    }
}
