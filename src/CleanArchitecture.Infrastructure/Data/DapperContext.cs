using System.Data;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CleanArchitecture.Infrastructure.Data;

public class DapperContext : IDapper, IDisposable
{
	private readonly string? _connectionString;
	private IDbConnection _connection;

	public DapperContext(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString(ConfigKey.DatabaseConnectString);
	}

	public IDbConnection CreateConnection()
	{
		if (_connection.State != ConnectionState.Open)
		{
			_connection = new NpgsqlConnection(_connectionString);
			_connection.Open();
		}
		return _connection;
	}

	public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null)
	{
		using var connection = CreateConnection();
		return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
	}

	public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
	{
		using var connection = CreateConnection();
		return await connection.QueryAsync<T>(sql, param);
	}

	public async Task<int> ExecuteAsync(string sql, object? param = null)
	{
		using var connection = CreateConnection();
		return await connection.ExecuteAsync(sql, param);
	}

	public void Dispose()
	{
		if (_connection.State == ConnectionState.Open)
		{
			_connection.Close();
			_connection.Dispose();
		}
	}
}