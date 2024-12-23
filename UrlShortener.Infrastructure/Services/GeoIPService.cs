namespace UrlShortener.Infrastructure.Services;
using UrlShortener.Application.Interfaces;
using System.Net;
using MaxMind.Db;

public class GeoIPService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private const string DatabasePath = "GeoLite2-City.mmdb"; // Adjust the path as necessary

        public GeoIPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DownloadGeoIPDatabaseAsync() {
            var url = "";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabasePath);
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);

                Console.WriteLine("GeoIP database downloaded successfully: " + filePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error downloading GeoIP database: " + ex.Message);
            }
        }

        public async Task<string> FindLocationByIPAsync(IPAddress userIP, CancellationToken cancellationToken)
        {
            // Ensure the database is downloaded before reading
            if (!File.Exists(DatabasePath))
            {
                await DownloadGeoIPDatabaseAsync();
            }

            using (var reader = new Reader(DatabasePath))
            {
                var location = reader.f
                return location;                
            }
        }

        // Schedule task to download the database weekly
        public void ScheduleDatabaseDownload()
        {
            var timer = new System.Timers.Timer(TimeSpan.FromDays(7).TotalMilliseconds);
            timer.Elapsed += async (sender, e) => await DownloadGeoIPDatabaseAsync();
            timer.Start();
        }

    public Task<string> FindLocationByIPAsync(string userIP, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

    public class GeoLocation
    {
        public City City { get; set; }
    }

    public class City
    {
        public Dictionary<string, string> Names { get; set; }
    }