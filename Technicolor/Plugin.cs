﻿using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;
using HarmonyLib;
using Heck;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Logging;
using JetBrains.Annotations;
using SiraUtil.Zenject;
using Technicolor.Installers;
using Technicolor.Settings;
using static Technicolor.TechnicolorController;

namespace Technicolor
{
    [Plugin(RuntimeOptions.DynamicInit)]
    internal class Plugin
    {
        private static readonly Harmony _harmonyInstance = new(HARMONY_ID);

        [UsedImplicitly]
        [Init]
        public Plugin(Logger pluginLogger, Config config, Zenjector zenjector)
        {
            Log.Logger = new HeckLogger(pluginLogger);
            TechnicolorConfig.Instance = config.Generated<TechnicolorConfig>();
            zenjector.Install<PlayerInstaller>(Location.Player);
        }

#pragma warning disable CA1822
        [UsedImplicitly]
        [OnEnable]
        public void OnEnable()
        {
            _harmonyInstance.PatchAll(typeof(Plugin).Assembly);
            BSMLSettings.instance.AddSettingsMenu("Technicolor", "Technicolor.Settings.settings.bsml", TechnicolorSettingsUI.instance);
            GameplaySetup.instance.AddTab("Technicolor", "Technicolor.Settings.modifiers.bsml", TechnicolorSettingsUI.instance);
            TechniModule.Enabled = true;
        }

        [UsedImplicitly]
        [OnDisable]
        public void OnDisable()
        {
            _harmonyInstance.UnpatchSelf();
            BSMLSettings.instance.RemoveSettingsMenu(TechnicolorSettingsUI.instance);
            GameplaySetup.instance.RemoveTab("Technicolor");
            TechniModule.Enabled = false;
            LightsEnabled = false;
            ObstaclesEnabled = false;
            NotesEnabled = false;
            BombsEnabled = false;
        }
#pragma warning restore CA1822
    }
}
