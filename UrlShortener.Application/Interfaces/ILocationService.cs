using System.Net;

namespace UrlShortener.Application.Interfaces;


public interface ILocationService {
    Task<string> FindLocationByIPAsync(IPAddress userIP, CancellationToken cancellationToken);
}