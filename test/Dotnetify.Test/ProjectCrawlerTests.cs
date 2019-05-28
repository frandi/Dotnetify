using Dotnetify.Test.Utils;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Dotnetify.Test
{
    public class ProjectCrawlerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("TestProject.csproj")]
        public async Task ParseCsproj_Success(string csprojFileName)
        {
            var projectLocation = await DummyProject.Generate("TestProject");

            var crawler = new ProjectCrawler();
            var dotnetProject = await crawler.ParseCsproj(Path.Combine(projectLocation, csprojFileName));

            await DummyProject.Clean("TestProject");

            Assert.Equal("TestProject", dotnetProject.Name);
            Assert.Equal("Microsoft.NET.Sdk.Web", dotnetProject.ProjectSdk);
        }
    }
}
