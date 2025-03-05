using System;
using UrlShortener.Application.Commands.Url;
using UrlShortener.Application.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Mappers;

public static partial class URLMapper {
  public static URLEntity ToURLEntity(this CreateURLCommand model) {
    return new()
    {
      FullURL = "",
      RedirectUrl = model.RedirectURL.Trim().EndsWith("/") == true ? model.RedirectURL.Substring(0,-1) : model.RedirectURL,
      Hash = model.Hash,
      AccountID = model.AccountID,
      DomainID = model.DomainID,
      IsActive = true,
      Description = null,
      StartDate = model.StartDate ?? new DateTime(),
      EndDate = model.EndDate ?? new DateTime().AddYears(1),
    };
  }

  public static URLDTO ToDTO(this URLEntity model) {
    return new URLDTO()
    {
      ID = model.ID,
      FullURL = string.IsNullOrEmpty(model.FullURL)?"":model.FullURL,
      Hash = model.Hash,
      DomainID = model.DomainID ?? 0,
    };
  }

  public static CreateURLResponse ToCreateURLResponse(this URLEntity model) {
    return new()
    {
      ID = model.ID,
      FullURL = model.FullURL ?? "",
      Hash = model.Hash,
      DomainID = model.DomainID ?? 0,
      CreatedAt = model.CreatedDate,
      StartDate = model.StartDate ?? new DateTime(),
      EndDate = model.EndDate ?? new DateTime().AddYears(1),
    };
  }
}
