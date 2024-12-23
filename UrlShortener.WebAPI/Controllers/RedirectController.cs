using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Queries;

namespace UrlShortener.WebAPI.Controllers;

[Microsoft.AspNetCore.Components.Route("")]
public class RedirectController {

  private readonly ISender _sender;
  
  public RedirectController(ISender sender) {
    _sender = sender;
  }
  
  [HttpGet("{hash}")]
  public async Task<RedirectResult> RedirectTraffic([FromQuery] string hash, [FromHeader] string host, HttpRequest request) {
    var senderModel = new GetTrafficRedirectQuery()
    {
      Host = host,
      Hash = hash,
      UserAgent = request.Headers.UserAgent.ToString(),
      RequestDate = new DateTime(),
      Headers = request.Headers.ToList()
    };
    var response = await _sender.Send(senderModel);
    return new RedirectResult(response.RedirectUrl, response.PermRedirect);
  }
  async redirectURL(@Param("hash") hash?: string, @Req() request?: any, @Res() response?: Response) {
    const domainName = request.headers["host"];
    const domainResult: Domain = await this.domainRepo.getDomainByName(domainName).getOne();

    let redirectURL = defaultURL
    if(domainResult) {
      redirectURL = domainResult.defaultLink?domainResult.defaultLink:defaultURL
    }

    if (hash && hash.trim() !== "" && domainResult) {
      const urlEntity = await this.urlRepo
        .getUrlByHash(hash)
        .andWhere("domainID = :domainID", { domainID: domainResult.id })
      .getOne();

      if(urlEntity) {
        let ipAddress:string;
        const userAgent = request.headers['user-agent'];
        // Check for IP
        if (request.headers["x-real-ip"]) {
          ipAddress = request.headers["x-real-ip"];
        } else {
          ipAddress = request.remoteAddress;
        }
       
        let urlRequestObjct = {
          "headers" : request.headers,
          "userAgent":userAgent,
          "hash": hash,
          "host": domainName,
          "urlEntityID": urlEntity.id,
          "ipAddress": ipAddress,
          "requestDate": moment().toString()
        }

        // let result  = await this.urlRequestDumpRepo.saveUrlRequestDump(urlRequestObjct);
        console.log("Request Dump Result: ", urlRequestObjct)
        await sendSnsTopic(urlRequestObjct,'/raw-request/'+domainName+'/'+hash, ['request'],urlEntity.id);
        
        if (redirectURL) {
          response.status(301)
          redirectURL = urlEntity.redirectURL          
        }

      }
    }

    response.redirect(redirectURL)
    return response;
  }
  
  @Get("/v2/:hash")
  async redirectPageV2(@Param("hash") hash?: string, @Req() request?: any, @Res() response?: Response) {

    const domainName = request.headers["host"];
    const domainResult: Domain = await this.domainRepo.getDomainByName(domainName).getOne();

    let redirectURL = defaultURL
    if(domainResult) {
      redirectURL = domainResult.defaultLink?domainResult.defaultLink:defaultURL
    }

    if (hash && hash.trim() !== "" && domainResult) {
      const urlEntity = await this.urlRepo
        .getUrlByHash(hash)
        .andWhere("domainID = :domainID", { domainID: domainResult.id })
        .getOne();

      if(urlEntity) {
        let ipAddress:string;
        const userAgent = request.headers['user-agent'];
          // Check for IP
        if (request.headers["x-real-ip"]) {
          ipAddress = request.headers["x-real-ip"];
        } else {
          ipAddress = request.remoteAddress;
        }
       
        let urlRequestObjct = {
          "headers" : request.headers,
          "userAgent":userAgent,
          "hash": hash,
          "host": domainName,
          "urlEntityID": urlEntity.id,
          "ipAddress": ipAddress,
          "requestDate": moment().toString()
        }

        // let result  = await this.urlRequestDumpRepo.saveUrlRequestDump(urlRequestObjct);
        console.log("Request Dump Result: ", urlRequestObjct)
        await sendSnsTopic(urlRequestObjct,'/raw-request/'+domainName+'/'+hash, ['request'],urlEntity.id);
        
        if (redirectURL) {
          response.status(301)
          redirectURL = urlEntity.redirectURL          
        }

      }
    }

    response.redirect(redirectURL)
    return response;
 
  }

  @Get("/v1/:hash")
  async redirectPage(@Param("hash") hash?: string, @Req() request?: any, @Res() response?: Response) {
    const domainName = request.headers["host"];
    const domainResult: Domain = await this.domainRepo.getDomainByName(domainName).getOne();
    let redirectURL = defaultURL
    if(domainResult) {
      redirectURL = domainResult.defaultLink?domainResult.defaultLink:defaultURL
    }
    if (hash && hash.trim() !== "" && domainResult) {
      const urlEntity = await this.urlRepo
        .getUrlByHash(hash)
        .andWhere("domainID = :domainID", { domainID: domainResult.id })
        .getOne();
      if(urlEntity) {

        let isUniqueClick: Boolean = true;
        let requestEntity: Request = new Request();
        let updateClicks: Url = new Url();
        const userAgent = request.useragent;
        const isUserABot: Boolean = await botsAndSpidersFiltering(userAgent.source);
        const currentDateAndTime: Date = new Date();        
       
        // Check for IP
        if (request.headers["x-real-ip"]) {
          requestEntity.ip = request.headers["x-real-ip"];
        } else {
          requestEntity.ip = request.connection.remoteAddress;
        }
        
        // Check if the click is unique
        const reqEntityWithSameIP = await this.reqRepo
        .getRequestByUrlId(urlEntity.id)
        .andWhere("request.ip = :reqIP", { reqIP: requestEntity.ip })
        .getOne();

         // Check if the click is unique
         if (reqEntityWithSameIP) {
          const date1 = moment(reqEntityWithSameIP.createdAt);
          const date2 = moment(currentDateAndTime);
          if (date2.diff(date1, "minutes") < 30) {
            isUniqueClick = false;
          }
        }

        if (isUserABot === false) {
          if (userAgent.browser !== "unknown" && userAgent.browser !== "curl") {
            if (isUniqueClick === true) {
              updateClicks.totalUniqueRequested = urlEntity.totalUniqueRequested + 1;
              requestEntity.isUnique = 1;
            } else {
              requestEntity.isUnique = 0;
            }
            
            // Save request into request table
            requestEntity.rawRequest = JSON.stringify(request.headers);
            requestEntity.payload = JSON.stringify({
              useragent: userAgent,
              headers: request.headers,
            });
            
            requestEntity.URLID = urlEntity.id;
            requestEntity.createdAt = currentDateAndTime;
            requestEntity.requestDate = currentDateAndTime;
            requestEntity.agentSource = userAgent.source;
            requestEntity.platform = userAgent.platform;
            requestEntity.os = userAgent.os;
            requestEntity.browser = userAgent.browser;
            requestEntity.browserVersion = userAgent.version;

            // Check for Referer
            if (request.headers["referer"]) {
              requestEntity.referrer = request.headers["referer"];
            } else {
              requestEntity.referrer = "Direct";
            }

            // Save Request Location
            
            if (requestEntity.ip) {
              let ipLong;
              let location;
              
              ipLong = ip.toLong(requestEntity.ip);

              const geoEntity = await this.geoRepo.getGeoWithInRange(ipLong);
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
                requestLocationEntity.createdAt = currentDateAndTime;
                requestLocationEntity.geoNameID = geoEntity.geoNameID;
                requestLocationEntity.geoCountryNameID = geoEntity.geoCountryNameID;

                const newRequestLocationEntry = await this.reqLocationRepo.saveNewRequestLocation(requestLocationEntity);
                // const newRequestLocationEntry = await this.reqLocationRepo.getLatestRequestLocationEntry(
                //   geoEntity.geoNameID,
                //   geoEntity.geoCountryNameID
                // );

                requestEntity.locationID = newRequestLocationEntry.id;
              }
            }

            await this.reqRepo.save(requestEntity)
          }

          // Update request info. in url table
          updateClicks.lastRequested = currentDateAndTime;
          updateClicks.totalRequested = urlEntity.totalRequested + 1;
          try {
            await this.urlRepo
              .getUrlById(urlEntity.id)
              .update()
              .set(updateClicks)
              .execute()
          } 
          catch(err) {
            console.log(err)
          }
        }
       
        if (redirectURL) {
          response.status(301)
          redirectURL = urlEntity.redirectURL          
        }
      }
    }
    response.redirect(redirectURL)
    return response;
  }
}