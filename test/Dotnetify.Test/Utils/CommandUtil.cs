using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Dotnetify.Test.Utils
{
    public static class CommandUtil
    {
        public static async Task<(string Output, string Error)> Execute(string fileName, string arguments)
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
