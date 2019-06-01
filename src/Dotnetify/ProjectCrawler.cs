using Dotnetify.Entities;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Dotnetify
{
    public class ProjectCrawler : IProjectCrawler
    {
        public async Task<string> GetCsprojFile(string projectLocation)
        {
            if (projectLocation.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
                return projectLocation;

            var files = await Task.Run(() => Directory.GetFiles(projectLocation, "*.csproj"));
            if (files.Length > 0)
                return files[0];

            return null;
        }

        public async Task<string> GetSlnFile(string rootFolder, bool searchSubFolders = true)
        {
            if (rootFolder.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase))
                return rootFolder;

            var files = await Task.Run(() => 
                    Directory.GetFiles(rootFolder, "*.sln", searchSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                );
            if (files.Length > 0)
                return files[0];

            return null;
        }

        public async Task<DotnetProject> ParseCsproj(string csprojLocation)
        {
            csprojLocation = await GetCsprojFile(csprojLocation);

            if (string.IsNullOrEmpty(csprojLocation) || !File.Exists(csprojLocation))
                return null;

            var csprojDoc = new XmlDocument();
            await Task.Run(() => csprojDoc.Load(csprojLocation));
            var projectRoot = csprojDoc.DocumentElement;

            var dotnetProject = new DotnetProject()
            {
                Name = Path.GetFileNameWithoutExtension(csprojLocation),
                ProjectSdk = projectRoot.GetAttribute("Sdk")
            };

            return dotnetProject;
        }
    }
}
