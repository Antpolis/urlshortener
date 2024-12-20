using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

public class DateTimeService: IDateTime
{
    public DateTime Now => DateTime.Now;
}