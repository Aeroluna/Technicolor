namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;
    using UnityEngine;

    [TechniPatch(typeof(ColorNoteVisuals))]
    [TechniPatch("HandleNoteControllerDidInit")]
    [TechniPatch(TechniPatchType.NOTES)]
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
