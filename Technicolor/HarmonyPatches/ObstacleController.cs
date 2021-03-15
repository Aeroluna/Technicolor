namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;

    [TechniPatch(typeof(ObstacleController))]
    [TechniPatch("Init")]
    [TechniPatch(TechniPatchType.OBSTACLES)]
    internal class ObstacleControllerInit
    {
        private static void Prefix(ObstacleController __instance, ObstacleData obstacleData)
        {
            __instance.SetObstacleColor(TechnicolorController.GetTechnicolor(true, obstacleData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorWallsStyle));
        }
    }
}
