using System;
using UrlShortener.Application.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Mappers;

public static partial class DomainMapper {
  public static DomainDTO ToDomainDTO(this DomainEntity model)
    {
        return new DomainDTO()
        {
            ID = model.ID,
            DomainURL = model.DomainURL!,
            DefaultLink = model.DefaultLink,
            CreatedAt = model.CreatedDate,
            UpdatedAt = model.LastModifiedDate
        };
    }
}
