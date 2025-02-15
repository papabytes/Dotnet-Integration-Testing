namespace Dotnet.Template.Application.CQSR;

public interface ICommand<R, P>
    where P: class
{
    Task<R> ExecuteAsync(P commandPayload);
}
