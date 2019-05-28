using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Dotnetify.Test.Utils
{
    public static class DummyProject
    {
        private static readonly string _tempLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "temp");
        public static async Task<string> Generate(string projectName)
        {
            var outputLocation = Path.Combine(_tempLocation, projectName);
            await CommandUtil.Execute("dotnet", $"new mvc -n {projectName} -o {outputLocation}");

            return outputLocation;
        }

        public static async Task Clean(string projectName)
        {
            var projectLocation = Path.Combine(_tempLocation, projectName);
            await Task.Run(() => Directory.Delete(projectLocation, true));
        }
    }
}
