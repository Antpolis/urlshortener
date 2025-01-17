using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Application.Queries;
using UrlShortener.Application.QueryExtensions;
using UrlShortener.Application.Responses;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Handlers;

public class GetTrafficRedirectHandler(IApplicationContext applicationContext, IHttpUserAgentParserProvider parser, ILocationService locationService)
    : IRequestHandler<GetTrafficRedirectQuery, GetTrafficRedirectResponse>
{
    private readonly IApplicationContext _applicationContext = applicationContext;
    private readonly IHttpUserAgentParserProvider _parser = parser;
    private readonly ILocationService _locationService = locationService;
    
    private string[] VALID_IP_HEADER_CANDIDATES = { 
        "X-Forwarded-For",
        "Proxy-Client-IP",
        "WL-Proxy-Client-IP",
        "HTTP_X_FORWARDED_FOR",
        "HTTP_X_FORWARDED",
        "HTTP_X_CLUSTER_CLIENT_IP",
        "HTTP_CLIENT_IP",
        "HTTP_FORWARDED_FOR",
        "HTTP_FORWARDED",
        "HTTP_VIA",
        "REMOTE_ADDR" };
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

    public async Task<GetTrafficRedirectResponse> Handle(GetTrafficRedirectQuery request, CancellationToken cancellationToken)
    {
        var domainModel = await _applicationContext.DomainEntity.AsQueryable().GetByDomain(request.Host).FirstOrDefaultAsync(cancellationToken);

        var returnResult = new GetTrafficRedirectResponse() {
            PermRedirect = false,
            RedirectUrl = "https://google.com"
        };        
        if(domainModel != null)
        {
            returnResult.RedirectUrl = domainModel.DefaultLink;
        }

        var urlModel = await _applicationContext.UrlEntity.AsQueryable().GetByHash(request.Hash)
            .Where(d => domainModel != null && d.DomainID == domainModel.ID)
            .FirstOrDefaultAsync(cancellationToken);
        
        if(urlModel != null)
        {
            returnResult.RedirectUrl = urlModel.RedirectUrl;
            returnResult.PermRedirect = true;
            
            var ipAddress = request.Headers
                .FirstOrDefault(header => VALID_IP_HEADER_CANDIDATES.Contains(header.Key))
                .Value.ToString() ?? "unknown";
            
            var createRequestCommand = request.ToCreateRequest(urlModel.ID, ipAddress);
        }

        return returnResult;
    }
}