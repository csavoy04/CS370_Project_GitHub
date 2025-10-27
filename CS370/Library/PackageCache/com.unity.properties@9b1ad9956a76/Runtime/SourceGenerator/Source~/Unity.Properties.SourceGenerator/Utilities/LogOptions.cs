using System;

namespace Unity.Properties.SourceGenerator
{
    [Flags]
    enum LogType
    {
        None = 0,
        PropertyBags = 1,
        PropertyBagRegistry = 2,
        Timings = 4,
        All = PropertyBags | PropertyBagRegistry | Timings
    }

    static class LogOptions
    {
        /// <summary>
        /// Sets the directory where the log files will be created.
        /// </summary>
        public static readonly string LogOutputDirectory = null;

        /// <summary>
        /// When <see cref="LogOutputDirectory"/> is set, this allows to control what information is logged on disk.
        /// </summary>
        public static readonly LogType LogType = LogType.All;
    }
}