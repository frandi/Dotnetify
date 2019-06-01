using Dotnetify.Entities;
using Dotnetify.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dotnetify
{
    public interface IDotnet
    {
        /// <summary>
        /// Add a project component
        /// </summary>
        /// <param name="template">Template of the component to create</param>
        /// <param name="name">Name of the component to create</param>
        /// <param name="outputPath">Relative path of the location to place the generated output</param>
        /// <param name="args">Additional arguments</param>
        /// <returns></returns>
        Task<Result> AddComponent(ComponentTemplate template, string name, string outputPath, Dictionary<string, string> args = null);

        /// <summary>
        /// Add a project to a solution
        /// </summary>
        /// <param name="projectPath">Relative path of the project (it could be the path to project folder or csproj file)</param>
        /// <param name="solutionPath">Relative path of the solution (it could be the path to solution folder or sln file)</param>
        /// <returns></returns>
        Task<Result> AddProjectToSolution(string projectPath, string solutionPath);

        /// <summary>
        /// Create a new .NET project
        /// </summary>
        /// <param name="template">Template of the project to create</param>
        /// <param name="name">Name of the project to create</param>
        /// <param name="outputPath">Relative path of the location to place the generated output</param>
        /// <param name="args">Additional arguments</param>
        /// <returns></returns>
        Task<Result<DotnetProject>> CreateProject(ProjectTemplate template, string name, string outputPath, Dictionary<string, string> args = null);

        /// <summary>
        /// Get a .NET project
        /// </summary>
        /// <param name="projectPath">Relative path of the project (it could be the path to project folder or csproj file)</param>
        /// <returns></returns>
        Task<DotnetProject> GetProject(string projectPath);

        /// <summary>
        /// Get installed .NET runtimes
        /// </summary>
        /// <returns></returns>
        Task<List<DotnetRuntime>> GetRuntimes();

        /// <summary>
        /// Get installed .NET SDKs
        /// </summary>
        /// <returns></returns>
        Task<List<DotnetSdk>> GetSdks();
    }
}
