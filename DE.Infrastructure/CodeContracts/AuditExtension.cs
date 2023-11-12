using DE.Infrastructure.Helpers;
using System;

namespace DE.Infrastructure.CodeContracts
{
    public static class AuditExtension
    {
        public static void ApplyAudit(this DomainModel domainModel, string auditToken)
        {
            domainModel.CreatedAt = DateTime.UtcNow;
            domainModel.UpdatedAt = DateTime.UtcNow;
            domainModel.CreatedBy = auditToken;
            domainModel.UpdatedBy = auditToken;
        }
    }
}
