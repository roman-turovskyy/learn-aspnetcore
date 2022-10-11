using Example.Common.Database;

namespace Example.Common.Messaging;

public class AuditAddCommand : ICommand
{
    public const string COMMAND_NAME = "cmd-global-audit-add";
    public string MessageName => COMMAND_NAME;
    public ICollection<AuditLogJson> AuditRecords { get; set; } = new List<AuditLogJson>();
}