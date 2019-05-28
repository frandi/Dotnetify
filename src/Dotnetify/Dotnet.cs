using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dotnetify
{
    public class Dotnet : IDotnet
    {
        private readonly DotnetOption _option;
        private readonly ICommandRunner _commandRunner;
        private readonly IProjectCrawler _projectCrawler;

        public Dotnet()
        {
            _option = new DotnetOption();
            _commandRunner = new CommandRunner();
            _projectCrawler = new ProjectCrawler();
        }

        public Dotnet(DotnetOption option, ICommandRunner commandRunner, IProjectCrawler projectCrawler)
        {
            _option = option;
            _commandRunner = commandRunner;
            _projectCrawler = projectCrawler;
        }

        public async Task<Result> AddComponent(ComponentTemplate template, string name, string outputPath, Dictionary<string, string> args = null)
        {
            var result = await CreateNew(template.ToString().ToLower(), name, outputPath, args);

            return new Result(result.Command);
        }

        public async Task<Result> AddProjectToSolution(string projectPath, string solutionPath)
        {
            var projectLocation = projectPath.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase)
                ? Path.Combine(_option.WorkingFolder, projectPath)
                : await _projectCrawler.GetCsprojFile(Path.Combine(_option.WorkingFolder, projectPath));

            var solutionLocation = solutionPath.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase)
                ? Path.Combine(_option.WorkingFolder, solutionPath)
                : await _projectCrawler.GetSlnFile(Path.Combine(_option.WorkingFolder, solutionPath));

            var command = $"dotnet sln \"{solutionLocation}\" add \"{projectLocation}\"";

            await _commandRunner.Run(command);

            return new Result(command);
        }

        public async Task<Result<DotnetProject>> CreateProject(ProjectTemplate template, string name, string outputPath, Dictionary<string, string> args = null)
        {
            var result = await CreateNew(template.ToString().ToLower(), name, outputPath, args);

            var csprojLocation = Path.Combine(result.OutputLocation, $"{name}.csproj");
            var returnValue = await _projectCrawler.ParseCsproj(csprojLocation);
            return new Result<DotnetProject>(result.Command, returnValue);
        }

        public async Task<DotnetProject> GetProject(string projectPath)
        {
            var csprojLocation = Path.Combine(_option.WorkingFolder, projectPath);
            if (!projectPath.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
            {
                csprojLocation = await _projectCrawler.GetCsprojFile(csprojLocation);
            }

            return await _projectCrawler.ParseCsproj(csprojLocation);
        }

        public async Task<List<DotnetRuntime>> GetRuntimes()
        {
            var command = $"dotnet --list-runtimes";
            var result = await _commandRunner.Run(command);

            var runtimes = new List<DotnetRuntime>();

            var splitted = result.Output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in splitted)
            {
                var arr = Regex.Split(item, @"\s(?![^[]*\])");
                runtimes.Add(new DotnetRuntime
                {
                    PackageName = arr[0],
                    Version = arr[1],
                    Location = arr[2]
                });
            }

            return runtimes;
        }

        public async Task<List<DotnetSdk>> GetSdks()
        {
            var command = $"dotnet --list-sdks";
            var result = await _commandRunner.Run(command);

            var sdks = new List<DotnetSdk>();

            var splitted = result.Output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in splitted)
            {
                var arr = Regex.Split(item, @"\s(?![^[]*\])");
                sdks.Add(new DotnetSdk
                {
                    Version = arr[0],
                    Location = arr[1]
                });
            }

            return sdks;
        }

        private async Task<(string Command, string OutputLocation)> CreateNew(string type, string name, string outputPath, Dictionary<string, string> args)
        {
            var outputLocation = Path.Combine(_option.WorkingFolder, outputPath);
            if (args != null)
            {
                if (args.ContainsKey("-o"))
                    outputLocation = args["-o"];
                if (args.ContainsKey("--output"))
                    outputLocation = args["--output"];
            }

            var command = $"dotnet new {type} -n \"{name}\" -o \"{outputLocation}\"";

            if (args != null)
            {
                var outputKeys = new[] { "-o", "--output" };
                foreach (var item in args)
                {
                    if (!outputKeys.Contains(item.Key))
                        command += $" {item.Key} {item.Value}";
                }
            }

            await _commandRunner.Run(command);

            return (command, outputLocation);
        }
    }
}
