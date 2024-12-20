using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Abstracts;

public abstract class QuerySearchParamsAbstract: IQuerySearchParams
{
    public int Limit { get; set; } = 15;
    public string? OrderBy { get; set; } = null;
    public int Page { get; set; } = 0;
    public string? SearchText { get; set; } = null;
}