using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Application.Queries;
using UrlShortener.Application.QueryExtensions;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Handlers;

public class GetTrafficRedirectHandler(IApplicationContext applicationContext)
    : IRequestHandler<GetTrafficRedirectQuery, GetTrafficRedirectResponse>
{
    private readonly IApplicationContext _applicationContext = applicationContext;
    
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

    public async Task<GetTrafficRedirectResponse> Handle(GetTrafficRedirectQuery request, CancellationToken cancellationToken)
    {
        var domainModel = await _applicationContext.DomainEntity.AsQueryable().GetByName(request.Host).FirstOrDefaultAsync(cancellationToken);

        var returnResult = new GetTrafficRedirectResponse();
        returnResult.PermRedirect = false;
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

            //
            // console.log("Request Dump Result: ", urlRequestObjct)
            // await sendSnsTopic(urlRequestObjct,'/raw-request/'+domainName+'/'+hash, ['request'],urlEntity.id);
            //
            // if (redirectURL) {
            //     response.status(301)
            //     redirectURL = urlEntity.redirectURL          
            // }
        }

        return returnResult;
    }
}