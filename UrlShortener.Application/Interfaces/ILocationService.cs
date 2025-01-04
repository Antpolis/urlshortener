using System.Net;

namespace UrlShortener.Application.Interfaces;


public interface ILocationService {
    Task<Dictionary<string, object>?> FindLocationByIPAsync(IPAddress userIP, CancellationToken cancellationToken);
}