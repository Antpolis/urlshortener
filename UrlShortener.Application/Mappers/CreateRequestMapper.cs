using System;
using System.Text.Json;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using UAParser;
using UrlShortener.Application.Commands;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Mappers;

public static partial class CreateRequestMapper {
  public static RequestEntity ToRequestEntity(this CreateRequestCommand model, HttpUserAgentInformation  userAgent) {
    return new() {
      URLID = model.UrlEntityID,
      AgentSource = model.UserAgent,
      RequestedDate = model.RequestDate,
      OS = userAgent.Platform != null ? userAgent.Platform.Value.Name:"unknown",
      Browser = userAgent.Name,
      BrowserVersion = userAgent.Version,
      Referrer = model.Headers.FirstOrDefault(d=>d.Key == "referer").Value.ToString() ?? "Direct",
      RawRequest = JsonSerializer.Serialize(model.Headers),
      IsUnique = false  
    };
  }
}
