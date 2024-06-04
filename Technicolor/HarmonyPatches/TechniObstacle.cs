using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Settings;

namespace Technicolor.HarmonyPatches
{
    internal class TechniObstacle : IAffinity
    {
        private readonly ObstacleColorizerManager _manager;
        private readonly Config _config;

        private TechniObstacle(ObstacleColorizerManager manager, Config config)
        {
            _manager = manager;
            _config = config;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
        private void Colorize(ObstacleController __instance, ObstacleData obstacleData)
        {
            _manager.Colorize(__instance, TechnicolorController.GetTechnicolor(
                true,
                obstacleData.time + __instance.GetInstanceID(),
                _config.TechnicolorWallsStyle));
        }
    }
}
