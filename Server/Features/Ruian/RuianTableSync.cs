using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Server.Infrastructure.Database;

namespace Server.Features.Ruian;

/// <summary>
/// Brings one RUIAN table in line with rows copied into a temporary staging table, changing only the rows that differ.
/// Needs an open transaction on the context; the staging table is dropped when it ends.
/// </summary>
public sealed class RuianTableSync
{
    private readonly NpgsqlConnection connection;
    private readonly NpgsqlTransaction transaction;
    private readonly IEntityType entityType;
    private readonly string table;
    private readonly string staging;
    private readonly string key;
    private readonly IReadOnlyList<string> valueColumns;

    public RuianTableSync(AppDbContext appDbContext, Type entityClrType)
    {
        entityType = appDbContext.Model.FindEntityType(entityClrType)
                     ?? throw new ArgumentException($"{entityClrType.Name} is not mapped.", nameof(entityClrType));
        connection = (NpgsqlConnection)appDbContext.Database.GetDbConnection();
        transaction = (NpgsqlTransaction?)appDbContext.Database.CurrentTransaction?.GetDbTransaction()
                      ?? throw new InvalidOperationException("Syncing a RUIAN table needs an open transaction.");
        table = entityType.GetTableName()
                ?? throw new InvalidOperationException($"{entityClrType.Name} has no table.");
        staging = "staging_" + table;
        key = entityType.FindPrimaryKey()!.Properties.Single().GetColumnName();
        valueColumns = entityType.GetProperties()
            .Where(p => !p.IsPrimaryKey())
            .Select(p => p.GetColumnName())
            .ToList();
    }

    /// <summary>Creates the empty staging table with the columns of the target.</summary>
    public async Task CreateStagingAsync(CancellationToken cancellationToken)
        => await ExecuteAsync($"CREATE TEMP TABLE {Quote(staging)} (LIKE {Quote(table)}) ON COMMIT DROP",
            cancellationToken);

    /// <summary>Starts a binary COPY into the staging table; rows must be written in the order of the given properties.</summary>
    public async Task<NpgsqlBinaryImporter> BeginCopyAsync(IReadOnlyList<string> propertyNames,
        CancellationToken cancellationToken)
    {
        var columns = string.Join(", ", propertyNames.Select(name => Quote(entityType.FindProperty(name)!.GetColumnName())));
        return await connection.BeginBinaryImportAsync(
            $"COPY {Quote(staging)} ({columns}) FROM STDIN (FORMAT BINARY)", cancellationToken);
    }

    /// <summary>Inserts the staged rows the target lacks and overwrites the ones that differ; returns how many.</summary>
    public async Task<int> UpsertAsync(CancellationToken cancellationToken)
    {
        var columns = string.Join(", ", valueColumns.Prepend(key).Select(Quote));
        var assignments = string.Join(", ", valueColumns.Select(c => $"{Quote(c)} = EXCLUDED.{Quote(c)}"));
        var current = string.Join(", ", valueColumns.Select(c => $"t.{Quote(c)}"));
        var staged = string.Join(", ", valueColumns.Select(c => $"EXCLUDED.{Quote(c)}"));
        return await ExecuteAsync(
            $"""
             INSERT INTO {Quote(table)} AS t ({columns})
             SELECT {columns} FROM {Quote(staging)}
             ON CONFLICT ({Quote(key)}) DO UPDATE SET {assignments}
             WHERE ({current}) IS DISTINCT FROM ({staged})
             """, cancellationToken);
    }

    /// <summary>Deletes the target rows that were not staged; returns how many.</summary>
    public async Task<int> DeleteMissingAsync(CancellationToken cancellationToken)
        => await ExecuteAsync(
            $"""
             DELETE FROM {Quote(table)} t
             WHERE NOT EXISTS (SELECT 1 FROM {Quote(staging)} s WHERE s.{Quote(key)} = t.{Quote(key)})
             """, cancellationToken);

    private async Task<int> ExecuteAsync(string sql, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.CommandTimeout = 0;
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string Quote(string identifier) => '"' + identifier.Replace("\"", "\"\"") + '"';
}
