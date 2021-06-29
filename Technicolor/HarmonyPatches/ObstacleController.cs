namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;

    [TechniPatch(typeof(ObstacleController))]
    [TechniPatch("Init")]
    [TechniPatch(TechniPatchType.OBSTACLES)]
    internal class ObstacleControllerInit
    {
        private static void Postfix(ObstacleController __instance, ObstacleData obstacleData)
        {
            __instance.ColorizeObstacle(TechnicolorController.GetTechnicolor(true, obstacleData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance!.TechnicolorWallsStyle));
        }
    }
}
