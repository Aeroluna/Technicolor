using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Managers;
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
            bool warm = noteData.colorType == ColorType.ColorA;

            void Technicolorize(TechnicolorStyle style)
            {
                if (style == TechnicolorStyle.OFF)
                {
                    return;
                }

                Color color = TechnicolorController.GetTechnicolor(
                    warm,
                    noteData.time + noteController.GetInstanceID(),
                    style);
                _manager.Colorize(noteController, color);
            }

            TechnicolorConfig config = TechnicolorConfig.Instance;
            bool useLeftStyle = config.UseLeftBlocksStyle;
            if (warm)
            {
                Technicolorize(useLeftStyle ? config.LeftTechnicolorBlocksStyle : config.TechnicolorBlocksStyle);
            }
            else
            {
                Technicolorize(config.TechnicolorBlocksStyle);
            }
        }
    }
}
