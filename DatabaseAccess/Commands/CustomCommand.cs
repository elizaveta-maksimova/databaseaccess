using DatabaseAccess.Commands.Abstract;
using DatabaseAccess.Commands.Models;

namespace DatabaseAccess.Commands
{
    public class CustomCommand : DatabaseCommand
    {
        public CustomCommand(string commandText, DatabaseParameter[] parameters)
        {
            FullCommandText = commandText;
            Parameters.AddRange(parameters);
        }

        public override string FullCommandText { get; }
    }
}
