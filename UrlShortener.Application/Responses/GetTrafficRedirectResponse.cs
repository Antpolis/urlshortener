namespace UrlShortener.Application.Responses;

public record GetTrafficRedirectResponse {
  public string RedirectUrl { get; set; } = "https://google.com";
  public bool PermRedirect { get; set; } = true;
}