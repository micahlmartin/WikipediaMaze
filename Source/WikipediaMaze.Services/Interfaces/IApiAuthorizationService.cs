namespace WikipediaMaze.Services.Interfaces
{
    public interface IApiAuthorizationService
    {
        bool IsAuthorized(string apiKey, string action);
    }
}