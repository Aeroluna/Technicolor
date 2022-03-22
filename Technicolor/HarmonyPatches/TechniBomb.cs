using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Settings;

namespace Technicolor.HarmonyPatches
{
    internal class TechniBomb : IAffinity
    {
        private readonly BombColorizerManager _manager;

        private TechniBomb(BombColorizerManager manager)
        {
            _manager = manager;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(BombNoteController), nameof(BombNoteController.Init))]
        private void Colorize(BombNoteController __instance, NoteData noteData)
        {
            _manager.Colorize(__instance, TechnicolorController.GetTechnicolor(
                true,
                noteData.time + __instance.GetInstanceID(),
                TechnicolorConfig.Instance.TechnicolorBombsStyle));
        }
    }
}
