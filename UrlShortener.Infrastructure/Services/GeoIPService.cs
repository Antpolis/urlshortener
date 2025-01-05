using MaxMind.GeoIP2;
using UrlShortener.Application.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Services;
using UrlShortener.Application.Interfaces;
using MaxMind.Db;
using System.Net;

public class GeoIPService : ILocationService {
    private readonly HttpClient _httpClient = new HttpClient();
    private const string CityDBName = "GeoLite2-City.mmdb";
    private string DatabaseCityPath {
        get {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CityDBName);
        }
    }
    private string DatabaseCountryPath {
        get {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeoLite2-Country.mmdb");
        }
    }
    private string DatabaseASNPath {
        get {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeoLite2-ASN.mmdb");
        }
    }

    public async Task DownloadGeoIPDatabaseAsync() {
        var url = "https://git.io/"+CityDBName;

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var filePath = Path.Combine(DatabaseCityPath);
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);

            Console.WriteLine("GeoIP database downloaded successfully: " + filePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error downloading GeoIP database: " + ex.Message);
        }
    }

    public async Task<GeoLocation?> FindLocationByIPAsync(IPAddress userIP, CancellationToken cancellationToken)
    {
        // Ensure the database is downloaded before reading
        if (!File.Exists(DatabaseCityPath))
        {
            await DownloadGeoIPDatabaseAsync();
        }

        using (var reader = new DatabaseReader(DatabaseCityPath))
        {
            var cityLocation = reader.City(userIP);
            return new GeoLocation()
            {
                ContinentCode = cityLocation.Continent.Code ?? null,
                ContinentName = cityLocation.Continent.Name ?? null,
                ISOCode = cityLocation.Country.IsoCode ?? null,
                CountryName = cityLocation.Country.Name ?? null,
                CityName = cityLocation.City.Name ?? null,
                PostalCode = cityLocation.Postal.Code ?? null,
                Latitude = cityLocation.Location.Latitude.HasValue?cityLocation.Location.Latitude.ToString():null,
                Longitude = cityLocation.Location.Longitude.HasValue?cityLocation.Location.Longitude.ToString():null,
                GeoNameId = cityLocation.City.GeoNameId
            };
        }
    }

    // Schedule task to download the database weekly
    public void ScheduleDatabaseDownload()
    {
        var timer = new System.Timers.Timer(TimeSpan.FromDays(7).TotalMilliseconds);
        timer.Elapsed += async (sender, e) => await DownloadGeoIPDatabaseAsync();
        timer.Start();
    }   
}
