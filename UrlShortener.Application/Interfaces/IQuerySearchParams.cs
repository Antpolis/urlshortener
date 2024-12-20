namespace UrlShortener.Application.Interfaces;

public interface IQuerySearchParams {
    public int Limit { get; set; }
    public string? OrderBy { get; set; }
    public int Page { get; set; }
    public string? SearchText { get; set; }
}