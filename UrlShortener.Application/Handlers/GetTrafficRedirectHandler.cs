using MediatR;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Queries;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Handlers;

public class GetTrafficRedirectHandler(IApplicationContext applicationContext)
    : IRequestHandler<GetTrafficRedirectQuery, URLEntity>
{
    private readonly IApplicationContext _applicationContext = applicationContext;

    public Task<URLEntity> Handle(GetTrafficRedirectQuery request, CancellationToken cancellationToken)
    {
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
    }
}