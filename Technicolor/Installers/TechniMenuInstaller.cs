using JetBrains.Annotations;
using Technicolor.Settings;
using Zenject;

namespace Technicolor.Installers
{
    [UsedImplicitly]
    internal class TechniMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TechnicolorSettingsUI>().AsSingle();
        }
    }
}
