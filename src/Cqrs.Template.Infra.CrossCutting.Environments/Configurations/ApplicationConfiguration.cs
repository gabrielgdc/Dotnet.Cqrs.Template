namespace Cqrs.Template.Infra.CrossCutting.Environments.Configurations;

public class ApplicationConfiguration
{
    public string Environment { get; set; }
    public bool LogsOnConsole { get; set; }
    public string GlobalErrorCode { get; set; }
    public string GlobalErrorMessage { get; set; }
    public string ConnectionString { get; set; }
    public string Schema { get; set; }
}
