namespace Infra.CrossCutting.Environments.BaseConfigurations;

/// <summary>
/// Represents a base class for configuration settings used by HTTP client proxies.
/// </summary>
/// <remarks>
/// This abstract class provides a foundation for defining configuration options for HTTP client proxies within your application.
/// Subclasses can inherit from this class and extend it with specific properties relevant to their proxy implementation.
/// </remarks>
public abstract class BaseHttpClientProxyConfiguration
{
    /// <summary>
    /// The base URL used by the HTTP client proxy for making requests.
    /// </summary>
    public string BaseUrl { get; set; }
}
