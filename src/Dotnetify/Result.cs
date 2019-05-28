namespace Dotnetify
{
    public class Result
    {
        public Result(string command)
        {
            Command = command;
        }

        /// <summary>
        /// Evaluated Dotnet command
        /// </summary>
        public string Command { get; set; }
    }

    public class Result<T>
    {
        public Result(string command, T returnValue)
        {
            Command = command;
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Evaluated Dotnet command
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Return value of the operation
        /// </summary>
        public T ReturnValue { get; set; }
    }
}
