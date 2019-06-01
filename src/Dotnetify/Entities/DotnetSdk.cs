namespace Dotnetify.Entities
{
    /// <summary>
    /// .NET SDK object
    /// </summary>
    public class DotnetSdk
    {
        /// <summary>
        /// Version of the SDK
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Location of the installed SDK
        /// </summary>
        public string Location { get; set; }
    }
}
