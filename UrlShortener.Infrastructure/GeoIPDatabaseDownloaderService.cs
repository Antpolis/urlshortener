using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UrlShortener.Infrastructure
{
    public class GeoIPDatabaseDownloaderService : IHostedService, IDisposable
    {
        private readonly ILogger<GeoIPDatabaseDownloaderService> _logger;
        private Timer _timer;
        private readonly HttpClient _httpClient;
        private const string DataDirectory = "app/data"; // Directory to store the databases

        public GeoIPDatabaseDownloaderService(ILogger<GeoIPDatabaseDownloaderService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Check if the databases exist, if not, download them
            if (!File.Exists(Path.Combine(DataDirectory, "GeoLite2-City.mmdb")) || 
                !File.Exists(Path.Combine(DataDirectory, "GeoLite2-ASN.mmdb")))
            {
                _logger.LogInformation("GeoIP databases not found. Starting initial download...");
                await DownloadGeoIPDatabaseAsync("GeoLite2-City.mmdb", "GeoLite2-City");
                await DownloadGeoIPDatabaseAsync("GeoLite2-ASN.mmdb", "GeoLite2-ASN");
            }

            // Calculate the initial delay until the next Saturday at 12 AM UTC+0
            var now = DateTime.UtcNow;
            var nextRun = now.AddDays((7 - (int)now.DayOfWeek) % 7).Date; // Next Saturday
            var initialDelay = nextRun - now;

            // Set the timer to run every week
            _timer = new Timer(DownloadDatabase, null, initialDelay, TimeSpan.FromDays(7));
        }

        private async void DownloadDatabase(object state)
        {
            try
            {
                await DownloadGeoIPDatabaseAsync("GeoLite2-City.mmdb", "GeoLite2-City");
                await DownloadGeoIPDatabaseAsync("GeoLite2-ASN.mmdb", "GeoLite2-ASN");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading GeoIP databases.");
            }
        }

        private async Task DownloadGeoIPDatabaseAsync(string fileName, string editionId)
        {


            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var filePath = Path.Combine(DataDirectory, fileName);
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);

                _logger.LogInformation($"{editionId} database downloaded successfully: " + filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading {editionId} database.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
} 