using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Dotnetify
{
    public class CommandRunner : ICommandRunner
    {
        public async Task<(string Output, string Error)> Run(string command)
        {
            var fileName = command.Substring(0, command.IndexOf(' '));
            var arguments = command.Substring(command.IndexOf(' '));

            return await Run(fileName, arguments);
        }

        public async Task<(string Output, string Error)> Run(string fileName, string arguments)
        {
            var outputBuilder = new StringBuilder();
            var error = "";

            var startInfo = new ProcessStartInfo(fileName, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        outputBuilder.AppendLine(line);

                        error = await process.StandardError.ReadToEndAsync();
                    }
                }
            }

            return (outputBuilder.ToString(), error);
        }
    }
}
