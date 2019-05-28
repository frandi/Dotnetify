namespace Dotnetify
{
    /// <summary>
    /// .NET Runtime object
    /// </summary>
    public class DotnetRuntime
    {
        /// <summary>
        /// Name of the runtime package
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// Version of the runtime package
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Location of the installed runtime package
        /// </summary>
        public string Location { get; set; }
    }
}
