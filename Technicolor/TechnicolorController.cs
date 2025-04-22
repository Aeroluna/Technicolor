using System.ComponentModel;
using System.Reflection;
using Heck.Module;
using Technicolor.Managers;
using UnityEngine;
using Random = System.Random;

namespace Technicolor
{
    internal static class TechnicolorController
    {
        internal const string HARMONY_ID = "aeroluna.Technicolor";

        internal static Color[] TechnicolorWarmPalette { get; } = { new(1, 0, 0), new(1, 0, 1), new(1, 0.6f, 0), new(1, 0, 0.4f) };

        internal static Color[] TechnicolorColdPalette { get; } = { new(0, 0.501f, 1), new(0, 1, 0), new(0, 0, 1), new(0, 1, 0.8f) };

        internal static Random TechniLightRandom { get; set; } = new(400);

        internal static bool TechnicolorEnabled { get; set; }

        internal static bool LightsEnabled { get; set; }

        internal static bool ObstaclesEnabled { get; set; }

        internal static bool NotesEnabled { get; set; }

        internal static bool BombsEnabled { get; set; }

        internal static Color GetTechnicolor(bool warm, float time, TechnicolorStyle style, TechnicolorTransition transition = TechnicolorTransition.FLAT)
        {
            return style switch
            {
                TechnicolorStyle.PURE_RANDOM => Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f),
                TechnicolorStyle.WARM_COLD => warm
                    ? GetWarmTechnicolour(time, transition)
                    : GetColdTechnicolour(time, transition),
                _ => throw new InvalidEnumArgumentException(nameof(style), (int)style, typeof(TechnicolorStyle))
            };
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
            return transition switch
            {
                TechnicolorTransition.FLAT => GetRandomFromArray(TechnicolorWarmPalette),
                TechnicolorTransition.SMOOTH => GetLerpedFromArray(TechnicolorWarmPalette, time),
                _ => Color.white
            };
        }

        private static Color GetColdTechnicolour(float time, TechnicolorTransition transition)
        {
            return transition switch
            {
                TechnicolorTransition.FLAT => GetRandomFromArray(TechnicolorColdPalette),
                TechnicolorTransition.SMOOTH => GetLerpedFromArray(TechnicolorColdPalette, time),
                _ => Color.white
            };
        }

        private static Color GetRandomFromArray(Color[] colors)
        {
            return GetLerpedFromArray(colors, (float)TechniLightRandom.NextDouble());
        }
    }
}
