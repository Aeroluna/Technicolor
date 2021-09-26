namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Heck;
    using Technicolor.Settings;

    [HeckPatch(typeof(ObstacleController))]
    [HeckPatch("Init")]
    [HeckPatch((int)TechniPatchType.OBSTACLES)]
    internal class ObstacleControllerInit
    {
        private static void Postfix(ObstacleController __instance, ObstacleData obstacleData)
        {
            __instance.ColorizeObstacle(TechnicolorController.GetTechnicolor(true, obstacleData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorWallsStyle));
        }
    }
}
