namespace Application.Common.Interfaces;

public interface IMeetingRequestRegistrationGuard
{
    Task<T> ExecuteAsync<T>(
        string hostNationalCode,
        Func<Task<T>> register,
        CancellationToken cancellationToken);
}
