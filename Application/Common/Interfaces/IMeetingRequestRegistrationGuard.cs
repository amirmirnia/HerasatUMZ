namespace Application.Common.Interfaces;

public interface IMeetingRequestRegistrationGuard
{
    Task<int> ExecuteAsync(
        string hostNationalCode,
        Func<Task<int>> register,
        CancellationToken cancellationToken);
}
