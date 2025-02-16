namespace Dotnet.Template.Core.Exceptions;

using System.Threading.Tasks.Dataflow;

public class CitizenIdAlreadyExistsException : Exception
{
    public CitizenIdAlreadyExistsException(string citizenId) : base ($"Person with Citizen ID = '{citizenId}' is already registered.")
    {
        
    }
}
