namespace Technicolor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using HarmonyLib;
    using UnityEngine;
    using static Technicolor.Plugin;

    internal static class TechnicolorController
    {
        private static List<TechniPatchData>? _lightPatches;
        private static List<TechniPatchData>? _obstaclePatches;
        private static List<TechniPatchData>? _notePatches;
        private static List<TechniPatchData>? _bombPatches;

        private static IDictionary<TechniPatchType, Harmony>? _techniPatchInstances;

        internal static Color[] TechnicolorWarmPalette { get; } = new Color[4] { new Color(1, 0, 0), new Color(1, 0, 1), new Color(1, 0.6f, 0), new Color(1, 0, 0.4f) };

        internal static Color[] TechnicolorColdPalette { get; } = new Color[4] { new Color(0, 0.501f, 1), new Color(0, 1, 0), new Color(0, 0, 1), new Color(0, 1, 0.8f) };

        internal static System.Random TechniLightRandom { get; private set; } = new System.Random(400);

        internal static void InitTechniPatches()
        {
            InitPatchType(ref _lightPatches, TechniPatchType.LIGHTS);
            InitPatchType(ref _obstaclePatches, TechniPatchType.OBSTACLES);
            InitPatchType(ref _notePatches, TechniPatchType.NOTES);
            InitPatchType(ref _bombPatches, TechniPatchType.BOMBS);

            if (_techniPatchInstances == null)
            {
                _techniPatchInstances = new Dictionary<TechniPatchType, Harmony>();
                foreach (TechniPatchType patchType in Enum.GetValues(typeof(TechniPatchType)))
                {
                    _techniPatchInstances.Add(patchType, new Harmony(HARMONYID + Enum.GetName(typeof(TechniPatchType), patchType)));
                }
            }
        }

        internal static void ToggleTechniPatches(bool value, TechniPatchType patchType)
        {
            if (value)
            {
                if (!Harmony.HasAnyPatches(HARMONYID + Enum.GetName(typeof(TechniPatchType), patchType)))
                {
                    List<TechniPatchData>? list = null;
                    switch (patchType)
                    {
                        case TechniPatchType.LIGHTS:
                            list = _lightPatches;
                            break;

                        case TechniPatchType.OBSTACLES:
                            list = _obstaclePatches;
                            break;

                        case TechniPatchType.NOTES:
                            list = _notePatches;
                            break;

                        case TechniPatchType.BOMBS:
                            list = _bombPatches;
                            break;
                    }

                    if (list != null)
                    {
                        Harmony harmony = _techniPatchInstances![patchType];
                        list.ForEach(n => harmony.Patch(
                            n.OriginalMethod,
                            n.Prefix != null ? new HarmonyMethod(n.Prefix) : null,
                            n.Postfix != null ? new HarmonyMethod(n.Postfix) : null,
                            n.Transpiler != null ? new HarmonyMethod(n.Transpiler) : null));
                    }
                }
            }
            else
            {
                _techniPatchInstances![patchType].UnpatchAll(HARMONYID + Enum.GetName(typeof(TechniPatchType), patchType));
            }
        }

        internal static void InitPatchType(ref List<TechniPatchData>? patchDatas, TechniPatchType patchType)
        {
            if (patchDatas == null)
            {
                patchDatas = new List<TechniPatchData>();
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    IEnumerable<object> attributes = type.GetCustomAttributes(typeof(TechniPatch), true);
                    if (attributes.Count() > 0)
                    {
                        Type? declaringType = null;
                        List<string> methodNames = new List<string>();
                        TechniPatchType? patchPatchType = null;
                        foreach (TechniPatch n in attributes)
                        {
                            if (n.DeclaringType != null)
                            {
                                declaringType = n.DeclaringType;
                            }

                            if (n.MethodName != null)
                            {
                                methodNames.Add(n.MethodName);
                            }

                            if (n.PatchType != null)
                            {
                                patchPatchType = n.PatchType;
                            }
                        }

                        if (declaringType == null || !methodNames.Any() || patchPatchType == null)
                        {
                            throw new ArgumentException("Type or Method Name not described");
                        }

                        if (patchPatchType == patchType)
                        {
                            MethodInfo prefix = AccessTools.Method(type, "Prefix");
                            MethodInfo postfix = AccessTools.Method(type, "Postfix");
                            MethodInfo transpiler = AccessTools.Method(type, "Transpiler");

                            foreach (string methodName in methodNames)
                            {
                                MethodInfo methodInfo = declaringType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                                if (methodInfo == null)
                                {
                                    throw new ArgumentException($"Could not find method '{methodName}' of '{declaringType}'");
                                }

                                patchDatas.Add(new TechniPatchData(methodInfo, prefix, postfix, transpiler));
                            }
                        }
                    }
                }
            }
        }

        internal static void ResetRandom()
        {
            TechniLightRandom = new System.Random(400);
        }

        internal static Color GetTechnicolor(bool warm, float time, TechnicolorStyle style, TechnicolorTransition transition = TechnicolorTransition.FLAT)
        {
            switch (style)
            {
                case TechnicolorStyle.PURE_RANDOM:
                    return Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f);

                case TechnicolorStyle.WARM_COLD:
                    return warm ? GetWarmTechnicolour(time, transition) : GetColdTechnicolour(time, transition);

                default: return Color.white;
            }
        }

        internal static Color GetLerpedFromArray(Color[] colors, float time)
        {
            float tm = Mathf.Repeat(time, colors.Length);
            int t0 = Mathf.FloorToInt(tm);
            int t1 = Mathf.CeilToInt(tm);
            if (t1 >= colors.Length)
            {
                t1 = 0;
            }

            return Color.Lerp(colors[t0], colors[t1], Mathf.Repeat(tm, 1));
        }

        private static Color GetWarmTechnicolour(float time, TechnicolorTransition transition)
        {
            switch (transition)
            {
                case TechnicolorTransition.FLAT:
                    return GetRandomFromArray(TechnicolorWarmPalette, time);

                case TechnicolorTransition.SMOOTH:
                    return GetLerpedFromArray(TechnicolorWarmPalette, time);

                default:
                    return Color.white;
            }
        }

        private static Color GetColdTechnicolour(float time, TechnicolorTransition transition)
        {
            switch (transition)
            {
                case TechnicolorTransition.FLAT:
                    return GetRandomFromArray(TechnicolorColdPalette, time);

                case TechnicolorTransition.SMOOTH:
                    return GetLerpedFromArray(TechnicolorColdPalette, time);

                default:
                    return Color.white;
            }
        }

        private static Color GetRandomFromArray(Color[] colors, float time, float seedMult = 8)
        {
            System.Random rand = new System.Random(Mathf.FloorToInt(seedMult * time));
            return colors[rand.Next(0, colors.Length)];
        }
    }
}
