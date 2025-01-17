using System;
using MediatR;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Commands.Url;

public class UpdateURLCommand: IRequest<URLDTO> {
    public ulong? ID { get; set; }
    public string FullURL {get;set;}
    public string Hash {get;set;}
    public uint DomainID {get;set;}
    public DateTime StartDate {get;set;}
    public DateTime EndDate {get;set;}
}
