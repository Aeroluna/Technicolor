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
        private readonly Config _config;

        private TechniNote(NoteColorizerManager manager, Config config)
        {
            _manager = manager;
            _config = config;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit))]
        private void Colorize(NoteController noteController)
        {
            NoteData noteData = noteController.noteData;
            bool warm = noteData.colorType == ColorType.ColorA;

            bool useLeftStyle = _config.UseLeftBlocksStyle;
            if (warm)
            {
                Technicolorize(useLeftStyle ? _config.LeftTechnicolorBlocksStyle : _config.TechnicolorBlocksStyle);
            }
            else
            {
                Technicolorize(_config.TechnicolorBlocksStyle);
            }

            return;

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
        }
    }
}
