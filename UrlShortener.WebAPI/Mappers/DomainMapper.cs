using System;
using UrlShortener.WebAPI.DTOs;

namespace UrlShortener.WebAPI.Mappers;

public static partial class DomainMapper {
  public static DomainDTO ToDomainDTO(this UrlShortener.Application.DTOs.DomainDTO model)
    {
        return new DomainDTO()
        {
            ID = model.ID,
            DomainURL = model.DomainURL!,
            DefaultLink = model.DefaultLink,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt ?? DateTime.MinValue
        };
    }
}
