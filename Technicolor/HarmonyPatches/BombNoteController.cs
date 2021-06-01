namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;

    [TechniPatch(typeof(BombNoteController))]
    [TechniPatch("Init")]
    [TechniPatch(TechniPatchType.BOMBS)]
    internal class BombControllerInit
    {
        private static void Postfix(BombNoteController __instance, NoteData noteData)
        {
            __instance.ColorizeBomb(TechnicolorController.GetTechnicolor(true, noteData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorBombsStyle));
        }
    }
}
