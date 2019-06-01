using Dotnetify.Entities;
using Dotnetify.Enums;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dotnetify.Test
{
    public class DotnetTests
    {
        private const string WorkingFolder = "/root/working";

        private readonly IDotnet _dotnet;
        private readonly Mock<ICommandRunner> _commandRunner;
        private readonly Mock<IProjectCrawler> _projectCrawler;

        public DotnetTests()
        {
            var option = new DotnetOption
            {
                WorkingFolder = WorkingFolder
            };

            _commandRunner = new Mock<ICommandRunner>();
            _projectCrawler = new Mock<IProjectCrawler>();

            _dotnet = new Dotnet(option, _commandRunner.Object, _projectCrawler.Object);
        }

        [Theory]
        [InlineData("dummy/location", "-na", "Dotnetify.Test")]
        [InlineData("", "-o", "/new/location")]
        public async Task AddComponent_Success(string outputPath, string argKey, string argValue)
        {
            var args = string.IsNullOrEmpty(argKey) ? null : new Dictionary<string, string> { { argKey, argValue } };
            var result = await _dotnet.AddComponent(ComponentTemplate.Page, "TestPage", outputPath, args);

            var outputLocation = argKey == "-o" ? argValue : Path.Combine(WorkingFolder, outputPath);
            var expectedCommand = $"dotnet new page -n \"TestPage\" -o \"{outputLocation}\"";
            if (!string.IsNullOrEmpty(argKey) && argKey != "-o")
            {
                expectedCommand += $" {argKey} {argValue}";
            }

            Assert.Equal(expectedCommand, result.Command);
        }

        [Theory]
        [InlineData("project/project.csproj", "solution.sln")]
        [InlineData("project", "")]
        public async Task AddProjectToSolution_Success(string projectPath, string solutionPath)
        {
            _projectCrawler.Setup(p => p.GetCsprojFile(It.IsAny<string>()))
                .ReturnsAsync((string loc) => Path.Combine(loc, "project.csproj"));
            _projectCrawler.Setup(p => p.GetSlnFile(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync((string root, bool subFolders) => Path.Combine(root, "solution.sln"));

            var result = await _dotnet.AddProjectToSolution(projectPath, solutionPath);

            var solutionLocation = string.IsNullOrEmpty(solutionPath) 
                ? Path.Combine(WorkingFolder, "solution.sln") 
                : Path.Combine(WorkingFolder, solutionPath);
            var projectLocation = projectPath.EndsWith(".csproj") 
                ? Path.Combine(WorkingFolder, projectPath) 
                : Path.Combine(WorkingFolder, projectPath, "project.csproj");
            var expectedCommand = $"dotnet sln \"{solutionLocation}\" add \"{projectLocation}\"";

            Assert.Equal(expectedCommand, result.Command);
        }

        [Theory]
        [InlineData("Project1", "src", "-au", "Individual")]
        [InlineData("Project2", "", "-o", "/new/location")]
        [InlineData("Project3", "", "", "")]
        public async Task CreateProject_Success(string projectName, string outputPath, string argKey, string argValue)
        {
            _projectCrawler.Setup(p => p.ParseCsproj(It.IsAny<string>()))
                .ReturnsAsync(new DotnetProject { Name = projectName });

            var args = string.IsNullOrEmpty(argKey) ? null : new Dictionary<string, string> { { argKey, argValue } };
            var result = await _dotnet.CreateProject(ProjectTemplate.Mvc, projectName, outputPath, args);

            var outputLocation = argKey == "-o" ? argValue : Path.Combine(WorkingFolder, outputPath);
            var expectedCommand = $"dotnet new mvc -n \"{projectName}\" -o \"{outputLocation}\"";
            if (!string.IsNullOrEmpty(argKey) && argKey != "-o")
            {
                expectedCommand += $" {argKey} {argValue}";
            }

            Assert.Equal(expectedCommand, result.Command);
            Assert.Equal(projectName, result.ReturnValue.Name);
        }

        [Theory]
        [InlineData("project")]
        [InlineData("project/project.csproj")]
        public async Task GetProject_Success(string projectPath)
        {
            _projectCrawler.Setup(p => p.GetCsprojFile(It.IsAny<string>()))
                .ReturnsAsync((string loc) => Path.Combine(loc, "project.csproj"));
            _projectCrawler.Setup(p => p.ParseCsproj(It.IsAny<string>()))
                .ReturnsAsync(new DotnetProject { Name = "project" });

            var result = await _dotnet.GetProject(projectPath);

            Assert.Equal("project", result.Name);
        }

        [Fact]
        public async Task GetRuntimes_Success()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"Microsoft.AspNetCore.All 2.2.5 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.All]");
            sb.AppendLine(@"Microsoft.AspNetCore.App 2.2.5 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]");
            sb.AppendLine(@"Microsoft.NETCore.App 2.2.5 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]");

            _commandRunner.Setup(c => c.Run(It.IsAny<string>())).ReturnsAsync((sb.ToString(), ""));

            var result = await _dotnet.GetRuntimes();

            Assert.NotEmpty(result);
            Assert.Equal("2.2.5", result[0].Version);
        }

        [Fact]
        public async Task GetSdks_Success()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"2.1.700 [C:\Program Files\dotnet\sdk]");
            sb.AppendLine(@"2.2.204 [C:\Program Files\dotnet\sdk]");
            sb.AppendLine(@"3.0.100-preview3-010431 [C:\Program Files\dotnet\sdk]");

            _commandRunner.Setup(c => c.Run(It.IsAny<string>())).ReturnsAsync((sb.ToString(), ""));

            var result = await _dotnet.GetSdks();

            Assert.NotEmpty(result);
            Assert.Equal("2.1.700", result[0].Version);
        }
    }
}
