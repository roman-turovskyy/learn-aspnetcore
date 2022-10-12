using Example.Common.Database;

namespace Example.Common.Messaging;

public class ReferralAuditAddCommand : ICommand
{
    public const string COMMAND_NAME = "cmd-global-referralAudit-add";
    public string MessageName => COMMAND_NAME;
    public IReadOnlyCollection<ReferralAuditRecord> AuditRecords { get; set; } = new List<ReferralAuditRecord>();
}