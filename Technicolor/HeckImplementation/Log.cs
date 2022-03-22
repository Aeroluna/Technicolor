using Heck;
using JetBrains.Annotations;

namespace Technicolor
{
    // its here cause its easier to type Log.Logger than HeckController.Logger
    internal static class Log
    {
        [UsedImplicitly]
        internal static HeckLogger Logger { get; set; } = null!;
    }
}
