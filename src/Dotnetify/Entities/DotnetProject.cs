namespace Dotnetify.Entities
{
    /// <summary>
    /// Object of the .NET project
    /// </summary>
    public class DotnetProject
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the project SDK
        /// </summary>
        public string ProjectSdk { get; set; }

        /// <summary>
        /// Target framework for the project
        /// </summary>
        public string TargetFramework { get; set; }
    }
}
