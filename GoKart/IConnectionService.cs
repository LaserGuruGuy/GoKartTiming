namespace GoKart
{
    public interface IConnectionService
    {
        string ClientKey { get; set; }

        string ServiceAddress { get; set; }

        string AccessToken { get; set; }
    }
}