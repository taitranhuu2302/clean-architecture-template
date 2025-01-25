using System.Data;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IDapper
{
	IDbConnection CreateConnection();
	Task<T?> QuerySingleAsync<T>(string sql, object? param = null);
	Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
	Task<int> ExecuteAsync(string sql, object? param = null);
}