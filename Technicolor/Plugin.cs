namespace Technicolor
{
    using System.Reflection;
    using BeatSaberMarkupLanguage.GameplaySetup;
    using BeatSaberMarkupLanguage.Settings;
    using HarmonyLib;
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

        [Init]
        public void Init(IPALogger logger, Config config)
        {
            TechniLogger.IPAlogger = logger;
            TechnicolorConfig.Instance = config.Generated<TechnicolorConfig>();
            TechnicolorController.InitTechniPatches();
        }

        [OnEnable]
        public void OnEnable()
        {
            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            GameplaySetup.instance.AddTab("Technicolor", "Technicolor.Settings.modifiers.bsml", TechnicolorSettingsUI.instance);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        [OnDisable]
        public void OnDisable()
        {
            _harmonyInstance.UnpatchAll(HARMONYID);
            GameplaySetup.instance.RemoveTab("Technicolor");
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene scene)
        {
            if (scene.name == "MenuViewControllers" && prevScene.name == "EmptyTransition")
            {
                BSMLSettings.instance.AddSettingsMenu("Technicolor", "Technicolor.Settings.settings.bsml", TechnicolorSettingsUI.instance);
            }

            TechnicolorController.ResetRandom();
        }
    }
}
