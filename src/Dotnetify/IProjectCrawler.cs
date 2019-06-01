using Dotnetify.Entities;
using System.Threading.Tasks;

namespace Dotnetify
{
    public interface IProjectCrawler
    {
        /// <summary>
        /// Get the .csproj file
        /// </summary>
        /// <param name="projectLocation">Location of the project</param>
        /// <returns></returns>
        Task<string> GetCsprojFile(string projectLocation);

        /// <summary>
        /// Get the .sln file
        /// </summary>
        /// <param name="rootFolder">Root folder to search</param>
        /// <param name="searchSubFolders">Search in the sub folders as well? (default to true)</param>
        /// <returns></returns>
        Task<string> GetSlnFile(string rootFolder, bool searchSubFolders = true);

        /// <summary>
        /// Parse the .csproj file into a <see cref="DotnetProject"/> object
        /// </summary>
        /// <param name="csprojLocation">Location of the .csproj file</param>
        /// <returns></returns>
        Task<DotnetProject> ParseCsproj(string csprojLocation);
    }
}
