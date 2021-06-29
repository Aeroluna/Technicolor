namespace Technicolor
{
    using System;
    using System.Reflection;

    public enum TechniPatchType
    {
        LIGHTS,
        OBSTACLES,
        NOTES,
        BOMBS,
    }

    internal struct TechniPatchData
    {
        internal TechniPatchData(MethodInfo orig, MethodInfo pre, MethodInfo post, MethodInfo tran)
        {
            OriginalMethod = orig;
            Prefix = pre;
            Postfix = post;
            Transpiler = tran;
        }

        internal MethodInfo OriginalMethod { get; }

        internal MethodInfo Prefix { get; }

        internal MethodInfo Postfix { get; }

        internal MethodInfo Transpiler { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class TechniPatch : Attribute
    {
        internal TechniPatch(Type declaringType)
        {
            DeclaringType = declaringType;
        }

        internal TechniPatch(string methodName)
        {
            MethodName = methodName;
        }

        internal TechniPatch(TechniPatchType patchType)
        {
            PatchType = patchType;
        }

        internal TechniPatch(Type declaringType, string methodName)
        {
            DeclaringType = declaringType;
            MethodName = methodName;
        }

        internal TechniPatchType? PatchType { get; }

        internal Type? DeclaringType { get; }

        internal string? MethodName { get; }
    }
}
