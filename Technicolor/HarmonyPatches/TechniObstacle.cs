using Chroma.Colorizer;
using SiraUtil.Affinity;
using Technicolor.Settings;

namespace Technicolor.HarmonyPatches
{
    internal class TechniObstacle : IAffinity
    {
        private readonly ObstacleColorizerManager _manager;

        private TechniObstacle(ObstacleColorizerManager manager)
        {
            _manager = manager;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
        private void Colorize(ObstacleController __instance, ObstacleData obstacleData)
        {
            _manager.Colorize(__instance, TechnicolorController.GetTechnicolor(
                true,
                obstacleData.time + __instance.GetInstanceID(),
                TechnicolorConfig.Instance.TechnicolorWallsStyle));
        }
    }
}
