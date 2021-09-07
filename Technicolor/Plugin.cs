namespace Technicolor
{
    using System.Reflection;
    using BeatSaberMarkupLanguage.GameplaySetup;
    using BeatSaberMarkupLanguage.Settings;
    using HarmonyLib;
    using Heck;
    using IPA;
    using IPA.Config;
    using IPA.Config.Stores;
    using Technicolor.Settings;
    using UnityEngine.SceneManagement;
    using IPALogger = IPA.Logging.Logger;

    public enum TechnicolorStyle
    {
        OFF,
        WARM_COLD,
        PURE_RANDOM,
        GRADIENT,
    }

    public enum TechnicolorTransition
    {
        FLAT,
        SMOOTH,
    }

    public enum TechnicolorLightsGrouping
    {
        STANDARD,
        ISOLATED_GROUP,
        ISOLATED,
    }

    [Plugin(RuntimeOptions.DynamicInit)]
    internal class Plugin
    {
        internal const string HARMONYID = "com.noodle.BeatSaber.Technicolor";
        private static readonly Harmony _harmonyInstance = new Harmony(HARMONYID);

#pragma warning disable CS8618
        internal static HeckLogger Logger { get; private set; }
#pragma warning restore CS8618

        [Init]
        public void Init(IPALogger pluginLogger, Config config)
        {
            Logger = new HeckLogger(pluginLogger);
            TechnicolorConfig.Instance = config.Generated<TechnicolorConfig>();
            TechnicolorController.InitTechniPatches();
        }

        [OnEnable]
        public void OnEnable()
        {
            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            BSMLSettings.instance.AddSettingsMenu("Technicolor", "Technicolor.Settings.settings.bsml", TechnicolorSettingsUI.instance);
            GameplaySetup.instance.AddTab("Technicolor", "Technicolor.Settings.modifiers.bsml", TechnicolorSettingsUI.instance);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        [OnDisable]
        public void OnDisable()
        {
            _harmonyInstance.UnpatchAll(HARMONYID);
            BSMLSettings.instance.RemoveSettingsMenu(TechnicolorSettingsUI.instance);
            GameplaySetup.instance.RemoveTab("Technicolor");
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene scene)
        {
            TechnicolorController.ResetRandom();
        }
    }
}
