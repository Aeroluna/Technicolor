using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Settings;

namespace Technicolor.HarmonyPatches
{
    internal class TechniBomb : IAffinity
    {
        private readonly BombColorizerManager _manager;
        private readonly Config _config;

        private TechniBomb(BombColorizerManager manager, Config config)
        {
            _manager = manager;
            _config = config;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(BombNoteController), nameof(BombNoteController.Init))]
        private void Colorize(BombNoteController __instance, NoteData noteData)
        {
            _manager.GlobalColorize(TechnicolorController.GetTechnicolor(
                true,
                noteData.time + __instance.GetInstanceID(),
                _config.TechnicolorBombsStyle));
        }
    }
}
