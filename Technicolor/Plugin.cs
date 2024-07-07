using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using JetBrains.Annotations;
using SiraUtil.Zenject;
using Technicolor.Installers;
using static Technicolor.TechnicolorController;
using Config = Technicolor.Settings.Config;

namespace Technicolor
{
    [Plugin(RuntimeOptions.DynamicInit)]
    internal class Plugin
    {
        private static readonly Harmony _harmonyInstance = new(HARMONY_ID);

        [UsedImplicitly]
        [Init]
        public Plugin(IPA.Config.Config config, Zenjector zenjector)
        {
            zenjector.Install<TechniAppInstaller>(Location.App, config.Generated<Config>());
            zenjector.Install<TechniMenuInstaller>(Location.Menu);
            zenjector.Install<TechniPlayerInstaller>(Location.Player);
        }

#pragma warning disable CA1822
        [UsedImplicitly]
        [OnEnable]
        public void OnEnable()
        {
            _harmonyInstance.PatchAll(typeof(Plugin).Assembly);
        }

        [UsedImplicitly]
        [OnDisable]
        public void OnDisable()
        {
            _harmonyInstance.UnpatchSelf();
            LightsEnabled = false;
            ObstaclesEnabled = false;
            NotesEnabled = false;
            BombsEnabled = false;
        }
#pragma warning restore CA1822
    }
}
