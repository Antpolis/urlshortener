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
    public async Task<CreateRequestResponse> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
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
          const geoEntity = await _locationService.FindLocationByIPAsync(ipAddress, cancellationToken);
          if (geoEntity) {
            let requestLocationEntity: RequestLocation = new RequestLocation();
            const geoName = await geoEntity.geoname;
            requestLocationEntity.continent = geoName.continent;
            requestLocationEntity.continentName = geoName.continentName;
            requestLocationEntity.ISOCode = geoName.ISOCode;
            requestLocationEntity.countryName = geoName.countryName;
            requestLocationEntity.cityName = geoName.cityName;
            requestLocationEntity.postalCode = geoEntity.postalCode;
            requestLocationEntity.latitude = geoEntity.latitude;
            requestLocationEntity.longitude = geoEntity.longitude;
            requestLocationEntity.createdAt = currentDateAndTime.toDate();
            requestLocationEntity.geoNameID = geoEntity.geoNameID;
            requestLocationEntity.geoCountryNameID = geoEntity.geoCountryNameID;
            console.log("saving location:", requestLocationEntity)
            const newRequestLocationEntry = await reqLocationRepo.saveNewRequestLocation(requestLocationEntity);
            requestEntity.locationID = newRequestLocationEntry.id;
        }

      }
  if(urlEntity){
    
    const currentDateAndTime = moment(request.requestDate)
    
    requestEntity.ip = request.ipAddress;
    

        // Save Request Location
        
        if (requestEntity.ip) {
          let ipLong;
          
          ipLong = toLong(requestEntity.ip);

          const geoEntity = await geoRepo.getGeoWithInRange(ipLong);
          if (geoEntity) {
            let requestLocationEntity: RequestLocation = new RequestLocation();
            const geoName = await geoEntity.geoname;
            requestLocationEntity.continent = geoName.continent;
            requestLocationEntity.continentName = geoName.continentName;
            requestLocationEntity.ISOCode = geoName.ISOCode;
            requestLocationEntity.countryName = geoName.countryName;
            requestLocationEntity.cityName = geoName.cityName;
            requestLocationEntity.postalCode = geoEntity.postalCode;
            requestLocationEntity.latitude = geoEntity.latitude;
            requestLocationEntity.longitude = geoEntity.longitude;
            requestLocationEntity.createdAt = currentDateAndTime.toDate();
            requestLocationEntity.geoNameID = geoEntity.geoNameID;
            requestLocationEntity.geoCountryNameID = geoEntity.geoCountryNameID;
            console.log("saving location:", requestLocationEntity)
            const newRequestLocationEntry = await reqLocationRepo.saveNewRequestLocation(requestLocationEntity);
            requestEntity.locationID = newRequestLocationEntry.id;
          }
        }
        try {

          const lastestRequestDetails = await reqRepo.save(requestEntity)
          
          if(lastestRequestDetails){
            await lastestRequestDetails.url;
            await lastestRequestDetails.requestLocation

            sendSnsTopic(lastestRequestDetails,'/request/'+request.host, ['create'], lastestRequestDetails.id);
          }
        } catch(e) {
          console.log(requestEntity)
          throw new Error(e);
        }
        
      }

      // Update request info. in url table
      
      updateClicks.lastRequested = currentDateAndTime.toDate();
      updateClicks.totalRequested = urlEntity.totalRequested + 1;
      try {
        console.log("Updating Unique Clicks:");
        console.log("UrlEntity :", urlEntity);
        console.log("Clicks:", updateClicks);
        
        await urlRepo.update(urlEntity.id, updateClicks)
        sendSnsTopic(urlEntity,'/url',['update'],urlEntity.id);
      } 
      catch(err) {
        console.log(updateClicks)
        throw new Error(err);
      }
    } else {
      console.log("Bot Detected: ", message.Body)
    }
  }
    }
}