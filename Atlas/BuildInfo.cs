using System;

using GameCore;

using Atlas.Enums;

namespace Atlas
{
    /// <summary>
    /// A tool to handle build info.
    /// </summary>
    public static class BuildInfo
    {
        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        public const string AssemblyVersion = "1.0.0";

        /// <summary>
        /// Gets the version.
        /// </summary>
        public static System.Version Version { get; } = typeof(BuildInfo).Assembly.GetName().Version;

        /// <summary>
        /// Gets the expected server version.
        /// </summary>
        public static string ExpectedServerVersion { get; } = "11.0.3";

        /// <summary>
        /// Gets the version in a string (including the build type).
        /// </summary>
        public static string VersionString => $"{Version.Major}.{Version.Minor}.{Version.Build}@{BuildType}";

        /// <summary>
        /// Gets the version in a string (including the build type and the beta type).
        /// </summary>
        public static string FullVersionString => IsForBeta ? VersionString + $":{BetaType}" : VersionString;

        /// <summary>
        /// Gets a vaĺue indicating whether this build is for the beta or not.
        /// </summary>
        public static bool IsForBeta { get; } = BetaType != BetaBranch.FullRelease;

        /// <summary>
        /// Gets the beta type if <see cref="IsForBeta"/> is true.
        /// </summary>
        public static BetaBranch BetaType { get; } = BetaBranch.FullRelease;

        /// <summary>
        /// Gets the build type.
        /// </summary>
        public static BuildType BuildType { get; } = BuildType.Alpha;

        /// <summary>
        /// Checks if the specified version is compatible with this version.
        /// </summary>
        /// <param name="other">The other version to check.</param>
        /// <returns>true if it's compatible, otherwise false.</returns>
        public static bool CheckVersion(this System.Version other)
            => other >= Version;

        /// <summary>
        /// Gets a value indicating whether this version of Atlas is compatible with the server version or not.
        /// </summary>
        /// <returns>true if it is compatible, otherwise false.</returns>
        public static bool IsCompatible()
            => GameCore.Version.VersionString == ExpectedServerVersion;
    }
}