namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;

    [TechniPatch(typeof(BombNoteController))]
    [TechniPatch("Init")]
    [TechniPatch(TechniPatchType.BOMBS)]
    internal class BombControllerInit
    {
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void Prefix(BombNoteController __instance, NoteData noteData)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            __instance.SetBombColor(TechnicolorController.GetTechnicolor(true, noteData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorBombsStyle));
        }
    }
}
