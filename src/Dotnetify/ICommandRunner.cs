using System.Threading.Tasks;

namespace Dotnetify
{
    public interface ICommandRunner
    {
        /// <summary>
        /// Run a command in a terminal
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <returns></returns>
        Task<(string Output, string Error)> Run(string command);

        /// <summary>
        /// Run a command in a terminal by specifying the program file name
        /// and its arguments.
        /// </summary>
        /// <param name="fileName">The program to run</param>
        /// <param name="arguments">Arguments of the program</param>
        /// <returns></returns>
        Task<(string Output, string Error)> Run(string fileName, string arguments);
    }
}
