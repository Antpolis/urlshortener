using System;
using MediatR;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Commands.Url;

public class GetURLQuery : IRequest<URLDTO> {
  public ulong ID { get; set; }
}
