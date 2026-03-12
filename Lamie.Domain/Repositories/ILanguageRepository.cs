namespace Lamie.Domain.Repositories;

public interface ILanguageRepository
{
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default);
}

