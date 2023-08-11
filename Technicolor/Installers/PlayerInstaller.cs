using System;
using JetBrains.Annotations;
using Technicolor.HarmonyPatches;
using Technicolor.Managers;
using Technicolor.Settings;
using Zenject;
using static Technicolor.TechnicolorController;

namespace Technicolor.Installers
{
    [UsedImplicitly]
    internal class PlayerInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (!TechnicolorEnabled)
            {
                return;
            }

            TechniLightRandom = new Random(400);
            Container.BindInterfacesTo<GradientController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BackgroundGradientColorizer>().AsSingle();

            if (LightsEnabled)
            {
                Container.BindInterfacesTo<TechniLights>().AsSingle();
                Container.BindInterfacesTo<TechniParticles>().AsSingle();
            }

            if (ObstaclesEnabled)
            {
                Container.BindInterfacesTo<TechniObstacle>().AsSingle();
            }

            if (NotesEnabled)
            {
                Container.BindInterfacesTo<TechniNote>().AsSingle();
            }

            if (BombsEnabled)
            {
                Container.BindInterfacesTo<TechniBomb>().AsSingle();
            }

            TechnicolorConfig config = TechnicolorConfig.Instance;
            if (config.TechnicolorSabersStyle != TechnicolorStyle.OFF ||
                (config.UseLeftSaberStyle && config.LeftTechnicolorSabersStyle != TechnicolorStyle.OFF))
            {
                Container.BindInstance(true).WithId("dontColorizeSabers");
            }
        }
    }
}
