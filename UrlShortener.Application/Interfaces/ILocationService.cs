using System.Net;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;


public interface ILocationService {
    Task<GeoLocation?> FindLocationByIPAsync(IPAddress userIP, CancellationToken cancellationToken);
}