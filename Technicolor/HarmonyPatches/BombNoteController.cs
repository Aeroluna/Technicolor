namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Heck;
    using Technicolor.Settings;

    [HeckPatch(typeof(BombNoteController))]
    [HeckPatch("Init")]
    [HeckPatch((int)TechniPatchType.BOMBS)]
    internal class BombControllerInit
    {
        private static void Postfix(BombNoteController __instance, NoteData noteData)
        {
            __instance.ColorizeBomb(TechnicolorController.GetTechnicolor(true, noteData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance!.TechnicolorBombsStyle));
        }
    }
}
