namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Heck;
    using Technicolor.Settings;
    using UnityEngine;

    [HeckPatch(typeof(ColorNoteVisuals))]
    [HeckPatch("HandleNoteControllerDidInit")]
    [HeckPatch((int)TechniPatchType.NOTES)]
    internal class ColorNoteVisualsHandleNoteControllerDidInitEventColorizer
    {
        private static void Postfix(NoteController noteController)
        {
            NoteData noteData = noteController.noteData;
            Color color = TechnicolorController.GetTechnicolor(noteData.colorType == ColorType.ColorA, noteData.time + noteController.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorBlocksStyle);
            noteController.ColorizeNote(color);
        }
    }
}
