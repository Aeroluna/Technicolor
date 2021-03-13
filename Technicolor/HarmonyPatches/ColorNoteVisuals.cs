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
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void Prefix(NoteController noteController)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            var noteData = noteController.noteData;
            var color = TechnicolorController.GetTechnicolor(noteData.colorType == ColorType.ColorA, noteData.time + noteController.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorBlocksStyle);
            noteController.SetNoteColors(color, color);
        }
    }
}
