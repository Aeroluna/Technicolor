using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Settings;
using UnityEngine;

namespace Technicolor.HarmonyPatches
{
    internal class TechniNote : IAffinity
    {
        private readonly NoteColorizerManager _manager;

        private TechniNote(NoteColorizerManager manager)
        {
            _manager = manager;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit))]
        private void Colorize(NoteController noteController)
        {
            NoteData noteData = noteController.noteData;
            Color color = TechnicolorController.GetTechnicolor(
                noteData.colorType == ColorType.ColorA,
                noteData.time + noteController.GetInstanceID(),
                TechnicolorConfig.Instance.TechnicolorBlocksStyle);
            _manager.Colorize(noteController, color);
        }
    }
}
