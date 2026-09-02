using System.Globalization;
using System.Net;
using Microsoft.Extensions.Options;

namespace Server.Features.Ruian;

/// <summary>Keeps the monthly ČÚZK address point zip in the data directory, downloading it only when needed.</summary>
public sealed class RuianAddressDownloader(
    HttpClient httpClient,
    IOptions<RuianOptions> options,
    IHostEnvironment environment,
    ILogger<RuianAddressDownloader> logger)
{
    /// <summary>Address of the whole-country zip; the date is the last day of the month the data is valid for.</summary>
    public const string ZipUrlFormat = "https://vdp.cuzk.gov.cz/vymenny_format/csv/{0:yyyyMMdd}_OB_ADR_csv.zip";

    private const string ZipSearchPattern = "*_OB_ADR_csv.zip";

    /// <summary>The newest zip on disk, or a freshly downloaded one when there is none or when forced.</summary>
    public async Task<(FileInfo Zip, bool Downloaded)> GetZipAsync(bool force, CancellationToken cancellationToken)
    {
        var directory = Directory.CreateDirectory(
            Path.Combine(environment.ContentRootPath, options.Value.AddressDataPath));

        if (!force)
        {
            var existing = directory.GetFiles(ZipSearchPattern).MaxBy(file => file.Name, StringComparer.Ordinal);
            if (existing is not null)
            {
                return (existing, false);
            }
        }

        return (await DownloadAsync(directory, cancellationToken), true);
    }

    private async Task<FileInfo> DownloadAsync(DirectoryInfo directory, CancellationToken cancellationToken)
    {
        // ČÚZK publishes the previous month's file on the 1st; when it is not there yet, the month before is used
        var today = DateTime.UtcNow;
        var monthEnd = new DateTime(today.Year, today.Month, 1).AddDays(-1);
        for (var attempt = 0;; attempt++)
        {
            var url = string.Format(CultureInfo.InvariantCulture, ZipUrlFormat, monthEnd);
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound && attempt == 0)
            {
                monthEnd = new DateTime(monthEnd.Year, monthEnd.Month, 1).AddDays(-1);
                continue;
            }

            response.EnsureSuccessStatusCode();

            var target = new FileInfo(Path.Combine(directory.FullName, Path.GetFileName(new Uri(url).AbsolutePath)));
            var partial = target.FullName + ".part";
            await using (var source = await response.Content.ReadAsStreamAsync(cancellationToken))
            await using (var file = File.Create(partial))
            {
                await source.CopyToAsync(file, cancellationToken);
            }

            File.Move(partial, target.FullName, overwrite: true);
            target.Refresh();
            logger.LogInformation("RUIAN address points downloaded: {File}, {Bytes} bytes", target.Name, target.Length);
            return target;
        }
    }
}
