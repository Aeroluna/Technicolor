using JetBrains.Annotations;
using Technicolor.Settings;
using Zenject;

namespace Technicolor.Installers
{
    [UsedImplicitly]
    internal class TechniAppInstaller : Installer
    {
        private readonly Config _config;

        private TechniAppInstaller(Config config)
        {
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config);
            Container.BindInterfacesTo<TechnicolorSettingsUI>().AsSingle();
            Container.BindInterfacesTo<TechnicolorModule>().AsSingle();
        }
    }
}
