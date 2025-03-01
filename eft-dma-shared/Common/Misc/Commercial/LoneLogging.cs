using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace eft_dma_shared.Common.Misc.Commercial
{
    public static class LoneLogging
    {
#pragma warning disable CS0649
        private static readonly Action<string> _writeLine;
#pragma warning restore CS0649

        static LoneLogging()
        {
            //if (AppContext.TryGetSwitch("LonesClient.Diagnostics.LoneLogging.Enabled", out bool enabled) && enabled)
            //{
            //    _writeLine = (Action<string>)AppContext.GetData("LonesClient.Diagnostics.LoneLogging.WriteLine");
            //}
        }

        /// <summary>
        /// Write a message to the log with a newline.
        /// </summary>
        /// <param name="data">Data to log. Calls .ToString() on the object.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLine(object data)
        {
            Debug.WriteLine(data);
            _writeLine?.Invoke(data.ToString()); // no-op'd if not enabled
        }
    }
}
