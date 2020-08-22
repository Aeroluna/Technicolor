namespace Technicolor.HarmonyPatches
{
    using Chroma.Colorizer;
    using Technicolor.Settings;

    [TechniPatch(typeof(ObstacleController))]
    [TechniPatch("Init")]
    [TechniPatch(TechniPatchType.OBSTACLES)]
    internal class ObstacleControllerInit
    {
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void Prefix(ObstacleController __instance, ObstacleData obstacleData)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            __instance.SetObstacleColor(TechnicolorController.GetTechnicolor(true, obstacleData.time + __instance.GetInstanceID(), TechnicolorConfig.Instance.TechnicolorWallsStyle));
        }
    }
}
