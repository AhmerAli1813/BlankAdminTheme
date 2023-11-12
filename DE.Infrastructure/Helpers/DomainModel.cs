using System;

namespace DE.Infrastructure.Helpers
{
    public interface ReadOnlyDomainModel
    {
        int Id { get; set; }
    }

    public interface DomainModel : ReadOnlyDomainModel
    {
        DateTime? CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
    }

}
