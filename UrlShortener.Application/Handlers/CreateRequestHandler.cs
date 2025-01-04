using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using UAParser;
using UrlShortener.Application.Commands;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Application.QueryExtensions;
using UrlShortener.Application.Responses;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Handlers;

public class CreateRequestHandler : IRequestHandler<CreateRequestCommand, CreateRequestResponse>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IHttpUserAgentParserProvider _parser;
  private readonly ILocationService _locationService;
  private readonly string[] BOT_USERAGENT = {
    "WhatsApp",
    "facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)",
    "facebookexternalhit/1.1",
    "Facebot",
    "spider",
    "jeeves",
    "crawler",
    "bot",
    "AHC",
    "dataminr.com"
  };
  public CreateRequestHandler(IApplicationContext applicationContext, IHttpUserAgentParserProvider parser, ILocationService locationService)
  {
    _applicationContext = applicationContext;
    _parser = parser;
    _locationService = locationService;
  }
  public async Task<CreateRequestResponse?> Handle(CreateRequestCommand request, CancellationToken cancellationToken) {
    var userAgent = _parser.Parse(request.UserAgent);
    var isBot = BOT_USERAGENT.Any(d=> request.UserAgent.IndexOf(d) >= 0) || userAgent.IsRobot();  
    if(isBot) {
      return null;
    }
    var urlModel = await _applicationContext.UrlEntity
      .ByActive()
      .Where(d=>d.ID == request.UrlEntityID)
      .FirstOrDefaultAsync(cancellationToken);
    if(urlModel != null) {
      var sameIPCount = await _applicationContext.RequestEntity
        .ByUrlID(urlModel.ID)
        .ByDateRange(request.RequestDate.AddMinutes(-15), request.RequestDate.AddMinutes(15))
        .Where(d=>d.IP == request.IPAddress)
        .CountAsync(cancellationToken);
      var requestModel = request.ToRequestEntity(userAgent);

      if(sameIPCount<=0) {
        requestModel.IsUnique = true;
      }
      
      if(IPAddress.TryParse(request.IPAddress, out IPAddress ipAddress)) {
        var geoEntity = await _locationService.FindLocationByIPAsync(ipAddress, cancellationToken);
        if (geoEntity != null) {
          var requestLocationModel = new RequestLocationEntity();
          // requestLocationModel.ContinentCode = geoEntity.get.continent;
          // requestLocationModel.ContinentName = geoName.continentName;
          // requestLocationModel.ISOCode = geoName.ISOCode;
          // requestLocationModel.CountryName = geoName.countryName;
          // requestLocationModel.CityName = geoName.cityName;
          // requestLocationModel.PostalCode = geoEntity.postalCode;
          // requestLocationModel.Latitude = geoEntity.latitude;
          // requestLocationModel.Longitude = geoEntity.longitude;
        }

     }
    
    }
    return null;
  }
}