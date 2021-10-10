namespace Technicolor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using HarmonyLib;
    using Heck;
    using UnityEngine;
    using static Technicolor.Plugin;

    internal enum TechniPatchType
    {
        LIGHTS,
        OBSTACLES,
        NOTES,
        BOMBS,
        FCKGRADIENTS,
    }

    internal static class TechnicolorController
    {
        private static IDictionary<TechniPatchType, Harmony>? _techniPatchInstances;
        private static IDictionary<TechniPatchType, bool>? _techniPatchActive;

        internal static Color[] TechnicolorWarmPalette { get; } = new Color[4] { new Color(1, 0, 0), new Color(1, 0, 1), new Color(1, 0.6f, 0), new Color(1, 0, 0.4f) };

        internal static Color[] TechnicolorColdPalette { get; } = new Color[4] { new Color(0, 0.501f, 1), new Color(0, 1, 0), new Color(0, 0, 1), new Color(0, 1, 0.8f) };

        internal static System.Random TechniLightRandom { get; private set; } = new System.Random(400);

        internal static void InitTechniPatches()
        {
            if (_techniPatchInstances == null)
            {
                _techniPatchInstances = new Dictionary<TechniPatchType, Harmony>();
                _techniPatchActive = new Dictionary<TechniPatchType, bool>();
                foreach (TechniPatchType patchType in Enum.GetValues(typeof(TechniPatchType)))
                {
                    _techniPatchActive.Add(patchType, false);

                    Harmony instance = new Harmony(HARMONYID + Enum.GetName(typeof(TechniPatchType), patchType));
                    _techniPatchInstances.Add(patchType, instance);
                    HeckData.InitPatches(instance, Assembly.GetExecutingAssembly(), (int)patchType);
                }
            }
        }

        internal static void ToggleTechniPatches(bool value, TechniPatchType patchType)
        {
            if (_techniPatchActive!.TryGetValue(patchType, out bool activeValue))
            {
                if (value != activeValue)
                {
                    HeckData.TogglePatches(_techniPatchInstances![patchType], value);

                    _techniPatchActive[patchType] = value;
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
                    return GetRandomFromArray(TechnicolorWarmPalette);

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
                    return GetRandomFromArray(TechnicolorColdPalette);

                case TechnicolorTransition.SMOOTH:
                    return GetLerpedFromArray(TechnicolorColdPalette, time);

                default:
                    return Color.white;
            }
        }

        private static Color GetRandomFromArray(Color[] colors)
        {
            return GetLerpedFromArray(colors, (float)TechniLightRandom.NextDouble());
        }
    }
}
